using BLL.Services.BillingService;
using BLL.Services.JobService;
using BLL.Services.UpgradeRecordService;
using Core.Enums;
using Core.Models;
using Core.ViewModels;
using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Presentation.SubSections.Tasks.RepairTask;
using Presentation.SubSections.Tasks.ServiceTask;
using Presentation.SubSections.Tasks.UpgradeTask;
using System;



namespace Presentation.SubSections.Jobs
{
    public partial class ViewJobSection : UserControl
    {
        private readonly IServiceProvider _serviceProvider;

        private int _jobId; // private field to store the job ID
        private int repairTaskId; // private field to store the repair task ID
        //private int upgradeTaskId; // private field to store the upgrade task ID
        private int upgradeTaskId;
        private int serviceTaskId; // private field to store the service task ID


        private Action _callback; // Callback for when a task is added or edited

        //user controls for each task types default view the grid view
        private GridViewRepairTask _repairTaskGridView;
        private GridViewUpgradeTask _upgradeTaskGridView;
        private GridViewServiceTask _serviceTaskGridView;



        //user controls for adding tasks
        private AddRepairTask _addrepairTask;
        private AddUpgradeTask _addUpgradeTask;
        private AddServiceTask _addServiceTask;

        //user controls for editing tasks
        private EditRepairTask _editRepairTask;
        private EditUpgradeTask _editUpgradeTask;
        private EditServiceTask _editServiceTask;


        //a dictionary to keep track of the current user control for each tabs dedicated panel
        private Dictionary<Guna2Panel, UserControl> _currenttaskControls = new Dictionary<Guna2Panel, UserControl>();





        public ViewJobSection(IServiceProvider serviceProvider, int jobid, Action callback)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _jobId = jobid; // Initialize jobId with the provided value

            // Initialize the task user controls
            initializeTaskUserControls();
            _callback = callback;

            // this.btnaddRepairTask.Click += (s, e) => ShowAddRepairTaskView(); // Event handler for the "+ Add Repair Task" button

        }



        private async void ViewJobSection_Load(object sender, EventArgs e)
        {
            //populate the job details when the section loads
            await LoadJob();

        }


        


        public async Task LoadJob()
        {

            try
            {
                JobDetailsViewModel jobDetails;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
                    // Call the NEW, EFFICIENT service method
                    jobDetails = await jobService.GetJobDetailsForViewByIdAsync(_jobId);
                }

                if (jobDetails == null)
                {
                    MessageBox.Show("Job details could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _callback?.Invoke(); // Go back if job not found
                    return;
                }

                // --- Populate Job and Device/Customer Details ---
                lbljobId.Text = jobDetails.JobId.ToString();
                lblDeviceName.Text = jobDetails.DeviceIdentifier;
                lblCustomerName.Text = jobDetails.CustomerFullName;
                lbljobSectionJobDescription.Text = jobDetails.JobDescription ?? "N/A";
                lblCompleted.Text = jobDetails.CompletionDate?.ToString("yyyy-MM-dd h:mm tt", System.Globalization.CultureInfo.InvariantCulture) ?? "Not Completed";
                lblCheckeIn.Text = jobDetails.CheckedInDate.ToString("yyyy-MM-dd h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                lblJobDetails.Text = $"Job Details: #{jobDetails.JobId}";

                // --- Populate Status ComboBox ---
                cmbboxJobStatus.DataSource = Enum.GetValues(typeof(Core.Enums.JobStatus));
                cmbboxJobStatus.SelectedItem = jobDetails.JobStatus;

                // --- Handle "Generate Invoice" Button Visibility ---
                bool isBillableStatus = jobDetails.JobStatus == Core.Enums.JobStatus.ReadyForBilling ||
                                        jobDetails.JobStatus == Core.Enums.JobStatus.Collected ||
                                        jobDetails.JobStatus == Core.Enums.JobStatus.TasksCompleted ||
                                        jobDetails.JobStatus == Core.Enums.JobStatus.ReadyForPickup;
                btnviewjobSectiongenerateInvoice.Visible = isBillableStatus;

                // --- Populate Billing & Cost Section ---
                txtboxManualJobCost.Text = jobDetails.ManualJobCost.ToString("F2");

                if (jobDetails.BillingExists)
                {
                    // Populate from the ViewModel's billing properties
                    txtboxBillingAmount.Text = jobDetails.BillingAmount.ToString("F2");
                    checkboxBillingViewIspaid.Checked = jobDetails.IsPaid;
                    datetimepickerbillPaymentDate.Value = jobDetails.PaymentDate ?? DateTime.Now;
                    cmbboxPaymentMethod.DataSource = Enum.GetValues(typeof(Core.Enums.PaymentMethods));
                    cmbboxPaymentMethod.SelectedItem = jobDetails.PaymentMethod;
                }
                else
                {
                    // No existing bill. Calculate the suggested amount.
                    decimal suggestedBillingAmount = jobDetails.TotalComponentChargeToCustomer + jobDetails.ManualJobCost;
                    txtboxBillingAmount.Text = suggestedBillingAmount.ToString("F2");

                    // Set defaults for other controls
                    checkboxBillingViewIspaid.Checked = false;
                    datetimepickerbillPaymentDate.Value = DateTime.Now;
                    cmbboxPaymentMethod.DataSource = Enum.GetValues(typeof(Core.Enums.PaymentMethods));
                    cmbboxPaymentMethod.SelectedIndex = 0;
                }

                // --- Calculate and Display Profit ---
                // Use the values directly from the ViewModel
                decimal profit = jobDetails.BillingAmount - jobDetails.ActualStoreCostForComponents;
                lblcalculatedProfit.Text = profit.ToString("C2"); // "C2" formats as currency

                // --- Refresh Child Grids ---
                if(_repairTaskGridView != null) await _repairTaskGridView.RefreshGridView(); // Refresh the Repair Task grid view
                if (_upgradeTaskGridView != null) await _upgradeTaskGridView.RefreshGridView(); // Refresh the Upgrade Task grid view
                if (_serviceTaskGridView != null) await _serviceTaskGridView.RefreshGridView(); // Refresh the Service Task grid view

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the job: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

       

        private void initializeTaskUserControls()
        {

            // Instantiate the default grid views for each task type
            _repairTaskGridView = new GridViewRepairTask(_serviceProvider, _jobId, ShowAddRepairTaskView, ShowEditRepairTaskView); // Pass jobId and callbacks
            _upgradeTaskGridView = new GridViewUpgradeTask(_serviceProvider, _jobId, ShowAddUpgradeTaskView, ShowEditUpgradeTaskView);
            _serviceTaskGridView = new GridViewServiceTask(_serviceProvider, _jobId, ShowAddServiceTaskView, ShowEditServiceTaskView);

            // Instantiate the "Add" forms
            // The callback for "Add" forms will be to switch back to their respective grid views
            _addrepairTask = new AddRepairTask(_serviceProvider, _jobId,  RefreshAndShowRepairTaskGrid /*()=>SwitchTaskView(panelrepairsectionTab,_repairTaskGridView)*/); // Pass jobId and callback
            _addUpgradeTask = new AddUpgradeTask(_serviceProvider, _jobId, RefreshAndShowUpgradeTaskGrid /*()=>SwitchTaskView(panelupgradeSectionTab,_upgradeTaskGridView)*/);
            _addServiceTask = new AddServiceTask(_serviceProvider, _jobId, RefreshAndShowServiceTaskGrid/*()=>SwitchTaskView(panelsericetaskSectionTab,_serviceTaskGridView)*/);


            // Instantiate the "Edit" forms
            _editRepairTask = new EditRepairTask(_serviceProvider, _jobId, repairTaskId,  RefreshAndShowRepairTaskGrid/*() => SwitchTaskView(panelrepairsectionTab, _repairTaskGridView)*/); // Pass jobId and callback
            _editUpgradeTask = new EditUpgradeTask(_serviceProvider, _jobId, upgradeTaskId, RefreshAndShowUpgradeTaskGrid /*() => SwitchTaskView(panelupgradeSectionTab, _upgradeTaskGridView)*/);
            _editServiceTask = new EditServiceTask(_serviceProvider, _jobId, serviceTaskId, RefreshAndShowServiceTaskGrid /*() => SwitchTaskView(panelsericetaskSectionTab, _serviceTaskGridView)*/);
            // Initially display the grid views in their respective tab panels
            // Assuming your Guna2Panels inside the TabPages are named:
            // gunaPanelRepairs, gunaPanelUpgrades, gunaPanelServices
            SwitchTaskView(panelrepairsectionTab, _repairTaskGridView);
            SwitchTaskView(panelupgradeSectionTab, _upgradeTaskGridView);
            SwitchTaskView(panelsericetaskSectionTab, _serviceTaskGridView);

        }

        // Generic method to switch UserControls within a specific tab's panel
        public void SwitchTaskView(Guna.UI2.WinForms.Guna2Panel hostPanel, UserControl newTaskControl)
        {
            if (hostPanel == null) return;

            // Remove current control from this specific panel if it exists
            if (_currenttaskControls.TryGetValue(hostPanel, out UserControl currentControl))
            {
                if (currentControl != null)
                {
                    hostPanel.Controls.Remove(currentControl);
                    // Optionally dispose 'currentControl' if it's not being reused and is IDisposable
                    // (currentControl as IDisposable)?.Dispose();
                }
            }

            // Add the new control
            newTaskControl.Dock = DockStyle.Fill;
            hostPanel.Controls.Add(newTaskControl);
            _currenttaskControls[hostPanel] = newTaskControl; // Update the current control for this panel
        }

        // --- Callbacks/Event Handlers from Task Grid Views to switch to "Add" or "Edit" views ---

        // Example for Repairs Tab
        private void ShowAddRepairTaskView()
        {
            // The _addRepairTaskSection should have a callback to switch back to _repairTaskGridView
            SwitchTaskView(panelrepairsectionTab, _addrepairTask);
        }

        public void ShowEditRepairTaskView(int repairTaskId) // Called from RepairTaskGridView
        {
            // Create EditRepairTaskSection on demand or reuse an instance
            var editRepairTaskSection = new EditRepairTask(_serviceProvider, _jobId, repairTaskId, RefreshAndShowRepairTaskGrid);
            SwitchTaskView(panelrepairsectionTab, editRepairTaskSection);
        }


        // Similar methods for Upgrades
        private void ShowAddUpgradeTaskView()
        {
            SwitchTaskView(panelupgradeSectionTab, _addUpgradeTask);
        }
        private void ShowEditUpgradeTaskView(int upgradeTaskId)
        {
            var editUpgradeTaskSection = new EditUpgradeTask(_serviceProvider, _jobId, upgradeTaskId,RefreshAndShowUpgradeTaskGrid);
            SwitchTaskView(panelupgradeSectionTab, editUpgradeTaskSection);
        }

        // Similar methods for Services
        private void ShowAddServiceTaskView()
        {
            SwitchTaskView(panelsericetaskSectionTab, _addServiceTask);
        }
        private void ShowEditServiceTaskView(int serviceTaskId)
        {
            var editServiceTaskSection = new EditServiceTask(_serviceProvider, _jobId, serviceTaskId, RefreshAndShowServiceTaskGrid);
            SwitchTaskView(panelsericetaskSectionTab, editServiceTaskSection);


        }


        private async void btnaddRepairTask_Click(object sender, EventArgs e)
        {
            //load add reapair task user control in the repair section tab panel

            //if the job status is tasks completed, collected, cancelled, ready for pickup or ready for billing, show an error message and return
            if (_jobId <= 0) // Ensure _jobId is valid
            {
                MessageBox.Show("Invalid Job ID.", "Error");
                return;
            }
            if (cmbboxJobStatus.SelectedItem == null || (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.TasksCompleted ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.Collected ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.Cancelled ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.ReadyForPickup ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.ReadyForBilling)
            {
                MessageBox.Show("Cannot add repair tasks when the job is Completed", "Error");
                return;
            }



            ShowAddRepairTaskView();
        }

        private async void btnaddUpgradeTask_Click(object sender, EventArgs e)
        {

            if (_jobId <= 0) // Ensure _jobId is valid
            {
                MessageBox.Show("Invalid Job ID.", "Error");
                return;
            }
            //if there is already an upgrade task for this job in the database, then we cannot add another upgrade task
            //get the job id and check if the job has any upgrade tasks
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
                if (await jobService.HasExistingUpgradeRecordsAsync(_jobId))
                {
                    MessageBox.Show("This job already has an active upgrade task. You can edit the existing task instead.", "Error");
                    return;
                }
            }



            if (cmbboxJobStatus.SelectedItem == null || (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.TasksCompleted ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.Collected ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.Cancelled ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.ReadyForPickup ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.ReadyForBilling)
            {
                MessageBox.Show("Cannot add upgrade tasks when the job is Completed", "Error");
                return;
            }


            ShowAddUpgradeTaskView();

        }

        private void btnaddServiceTask_Click(object sender, EventArgs e)
        {
            if (_jobId <= 0) // Ensure _jobId is valid
            {
                MessageBox.Show("Invalid Job ID.", "Error");
                return;
            }
            if (cmbboxJobStatus.SelectedItem == null || (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.TasksCompleted ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.Collected ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.Cancelled ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.ReadyForPickup ||
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem == Core.Enums.JobStatus.ReadyForBilling)
            {
                MessageBox.Show("Cannot add service tasks when the job is Completed", "Error");
                return;
            }


            ShowAddServiceTaskView();
        }

        private async void btnjobviewSectionsaveBillingInfo_Click(object sender, EventArgs e)
        {

            //this totalcomponentcost from the textbox is commented out because it should be calculated based on the components used in the job not from the textbox
            //if (!decimal.TryParse(txtboxBillingAmount.Text, out decimal totalcomponentcost))
            //{
            //    MessageBox.Show("Invalid Billing Amount.", "Error"); return;
            //}
            //validate before saving the billing information
            //if the job status is not ReadyForBilling, then we cannot save the billing information
            if (_jobId <= 0) // Ensure _jobId is valid
            {
                MessageBox.Show("Invalid Job ID.", "Error");
                return;
            }
            if (cmbboxJobStatus.SelectedItem == null || (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem != Core.Enums.JobStatus.ReadyForBilling)
            {
                MessageBox.Show("Job must be in 'Ready for Billing' status to save billing information.", "Error");
                return;
            }



            if (!decimal.TryParse(txtboxManualJobCost.Text, out decimal manualJobCost)) // Save updated manual cost
            {
                MessageBox.Show("Invalid Manual Job Cost.", "Error"); return;
            }

            // Assuming cmbboxPaymentMethod is populated with PaymentMethods enum
            Core.Enums.PaymentMethods paymentMethod = (Core.Enums.PaymentMethods)cmbboxPaymentMethod.SelectedItem;
            bool isPaid = checkboxBillingViewIspaid.Checked;
            DateTime? paymentDate = isPaid ? datetimepickerbillPaymentDate.Value : (DateTime?)null;


            //calculate the total component cost based on the components used in the job
            // --- 2. Re-calculate the component cost and total billing amount ---
            //    This is crucial to ensure we're not using a stale value from the textbox.
            //    We need to fetch the job with all its components to do this.
            decimal totalComponentChargeToCustomer = 0;
            try
            {


                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
                    // Use the efficient "Get all details" method

                   
                    // *** CALL THE NEW, FAST METHOD ***
                     totalComponentChargeToCustomer = await jobService.GetTotalComponentChargeForJobAsync(_jobId);

                    //this was the old way, but it was slow because it fetched all details now gettotalcomponentchargeforjobasync is fast and efficient because the total component cost is now calculated in the backend without fetching the whole job object.

                    //var jobWithDetails = await jobService.getjob(_jobId);

                    //if (jobWithDetails == null)
                    //{
                    //    MessageBox.Show("Could not find job to save billing.", "Error");
                    //    return;
                    //}
                    //no need to calculate the components cost here 
                    //// Sum component charges
                    //if (jobWithDetails.RepairRecords != null)
                    //    totalComponentChargeToCustomer += jobWithDetails.RepairRecords.SelectMany(rr => rr.RepairRecordComponents).Sum(rrc => rrc.QuantityUsed * rrc.PricePerUnit);

                    //if (jobWithDetails.UpgradeRecords != null)
                    //    totalComponentChargeToCustomer += jobWithDetails.UpgradeRecords.SelectMany(ur => ur.UpgradeRecordComponents).Sum(urc => urc.QuantityUsed * urc.PricePerUnit);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not calculate component costs: {ex.Message}", "Error");
                return;
            }

            // This is the FINAL, authoritative billing amount we want to save
            decimal finalBillingAmount = totalComponentChargeToCustomer + manualJobCost;

            if (finalBillingAmount <= 0)
            {
                var result = MessageBox.Show("The total billing amount is zero or less. Are you sure you want to proceed?", "Confirm Zero Bill", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            // Optional: You can update the textbox one last time just to be sure,
            // though it should already match if your TextChanged event works correctly.
            txtboxBillingAmount.Text = finalBillingAmount.ToString("F2");


            //call the service to save the billing information
            try
            {

                SetBillingControlsEnabled(false); // Disable controls while saving to prevent multiple clicks

                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<JobService>(); // Or JobService
                                                                                             // The service needs to either create a new Billing record or update an existing one for this _jobId
                    await jobService.SaveOrUpdateJobBillingAsync(
                        _jobId,
                        finalBillingAmount, // This is the final amount the user might have manually adjusted in the textbox
                        manualJobCost, // Save the manual job cost back to the Job entity
                        isPaid,
                        paymentDate,
                        paymentMethod
                    );
                    MessageBox.Show("Billing information saved successfully!", "Success");
                    await LoadJob();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving billing information: {ex.Message}", "Error");
            }
            finally
            {
                SetBillingControlsEnabled(true);
            }

        }

        // If each tab page has its own "Add [Type] Task" button (e.g., btnAddRepairInTab, btnAddUpgradeInTab)
        // you would wire their Click events to the respective ShowAdd...View methods.
        // E.g., in your ViewJobSection.Designer.cs (or via properties window):
        // this.btnAddRepairInTabPage.Click += (s,ev) => ShowAddRepairTaskView();


        //private async Task CalculateAndDisplayBillingAndProfitAsync(Job job = null) // Pass job to avoid re-fetching if already loaded
        //{
        //    if (job == null) // If not passed, fetch it again with all details
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
        //            job = await jobService.GetJob(_jobId);
        //            if (job == null) return; // Or handle error
        //        }
        //    }

        //    decimal totalComponentCost = 0;

        //    // Sum component costs from RepairRecords
        //    foreach (var rr in job.RepairRecords ?? Enumerable.Empty<RepairRecord>())
        //    {
        //        foreach (var rrc in rr.RepairRecordComponents ?? Enumerable.Empty<RepairRecordComponent>())
        //        {
        //            totalComponentCost += rrc.QuantityUsed * rrc.PricePerUnit; // Using PricePerUnit captured at time of use
        //        }
        //    }

        //    // Sum component costs from UpgradeRecords
        //    foreach (var ur in job.UpgradeRecords ?? Enumerable.Empty<UpgradeRecord>())
        //    {
        //        foreach (var urc in ur.UpgradeRecordComponents ?? Enumerable.Empty<UpgradeRecordComponent>())
        //        {
        //            totalComponentCost += urc.QuantityUsed * urc.PricePerUnit; // Using PricePerUnit captured at time of use
        //        }
        //    }

        //    // Sum component costs for service records is not required because it is not related with components


        //    decimal manualJobCost = decimal.TryParse(txtboxManualJobCost.Text, out decimal mjc) ? mjc : (job.ManualJobCost ?? 0);
        //    // Update the ManualJobCost textbox if it was loaded from the job directly
        //    if (txtboxManualJobCost.Text != manualJobCost.ToString("F2"))
        //    {
        //        txtboxManualJobCost.Text = manualJobCost.ToString("F2");
        //    }


        //    decimal calculatedBillingAmount = totalComponentCost + manualJobCost;
        //    txtboxBillingAmount.Text = calculatedBillingAmount.ToString("F2"); // Display calculated amount

        //    decimal profit = calculatedBillingAmount - manualJobCost - totalComponentCost; // This simplifies to totalComponentCost cancellation.
        //                                                                                   // Profit should be BillingAmount (what customer pays) - All Store Costs (ManualJobCost + totalComponentCost)
        //    profit = calculatedBillingAmount - totalComponentCost;
            
        //    // Calculated total charge to customer for components:
        //    decimal totalComponentChargeToCustomer = 0;


        //    if (job.RepairRecords != null)
        //    {
        //        foreach (var rr in job.RepairRecords)
        //        {
        //            if (rr.RepairRecordComponents != null)
        //            {
        //                foreach (var rrc in rr.RepairRecordComponents)
        //                {
        //                    totalComponentChargeToCustomer += rrc.QuantityUsed * rrc.PricePerUnit;
        //                }
        //            }
        //        }
        //    }
        //    if (job.UpgradeRecords != null)
        //    {
        //        foreach (var ur in job.UpgradeRecords)
        //        {
        //            if (ur.UpgradeRecordComponents != null)
        //            {
        //                foreach (var urc in ur.UpgradeRecordComponents)
        //                {
        //                    totalComponentChargeToCustomer += urc.QuantityUsed * urc.PricePerUnit;
        //                }
        //            }
        //        }
        //    }
        //    // ... add service records if they use components ...

        //    calculatedBillingAmount = totalComponentChargeToCustomer + manualJobCost;
        //    txtboxBillingAmount.Text = calculatedBillingAmount.ToString("F2");

        //    // To calculate profit, we need the store's *actual cost* of components used.
        //    decimal actualStoreCostForComponents = 0;
        //    if (job.RepairRecords != null)
        //    {
        //        foreach (var rr in job.RepairRecords)
        //        {
        //            if (rr.RepairRecordComponents != null)
        //            {
        //                foreach (var rrc in rr.RepairRecordComponents)
        //                {
        //                    // Ensure rrc.Component is loaded when Job is fetched by GetJobByIdWithAllDetailsForBillingAsync
        //                    actualStoreCostForComponents += rrc.QuantityUsed * (rrc.Component?.Price ?? 0m); // Use 0m as default if Component or Price is null
        //                }
        //            }
        //        }
        //    }
        //    if (job.UpgradeRecords != null)
        //    {
        //        foreach (var ur in job.UpgradeRecords)
        //        {
        //            if (ur.UpgradeRecordComponents != null)
        //            {
        //                foreach (var urc in ur.UpgradeRecordComponents)
        //                {
        //                    actualStoreCostForComponents += urc.QuantityUsed * (urc.Component?.Price ?? 0m);
        //                }
        //            }
        //        }
        //    }

        //    profit = calculatedBillingAmount - actualStoreCostForComponents;
        //    lblcalculatedProfit.Text = profit.ToString("C2"); // Display profit
        //}





        private async void btnjobsectionUpdateStatus_Click(object sender, EventArgs e)
        {


            if (cmbboxJobStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a job status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // This is the new status the user wants to set
            var newStatus = (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem;

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<JobService>();

                    // --- Business Logic for Status Transition ---
                    // Leting the service handle the update logic. just pass the new status.
                    await jobService.UpdateJobStatusAsync(_jobId, newStatus);

                    MessageBox.Show("Job status updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload the job to reflect all changes, especially calculated ones and button visibility
                    await LoadJob();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update job status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ////update the status of the job based on the selected status in the combobox
            //if (cmbboxJobStatus.SelectedItem == null)
            //{
            //    MessageBox.Show("Please select a job status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            ////get the current job from the service
            //using (var scope = _serviceProvider.CreateScope())
            //{
            //    var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
            //    var job = await jobService.GetJob(_jobId); // Get the job by ID
            //    if (job == null)
            //    {
            //        MessageBox.Show("Job not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    // Update the job status
            //    job.JobStatus = (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem;
            //    job.CompletionDate = job.JobStatus == Core.Enums.JobStatus.ReadyForPickup ? DateTime.Now : (DateTime?)null; // Set completion date if status is ReadyForPickup
            //    // Save the updated job
            //    jobService.UpdateJob(_jobId, job);
            //    MessageBox.Show("Job status updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

        }



        private void SetBillingControlsEnabled(bool isEnabled)
        {
            // Disable all controls in the billing section
            txtboxBillingAmount.Enabled = isEnabled;
            txtboxManualJobCost.Enabled = isEnabled;
            cmbboxPaymentMethod.Enabled = isEnabled;
            datetimepickerbillPaymentDate.Enabled = isEnabled;
            checkboxBillingViewIspaid.Enabled = isEnabled;
            btnjobviewSectionsaveBillingInfo.Enabled = isEnabled;
            // You might also want to show a "Saving..." label or a progress indicator
        }







        private void btnbacktoJobList_Click(object sender, EventArgs e)
        {
            //back to the job list section
            _callback?.Invoke(); // Invoke the callback to return to the job list section
        }

        private void btnviewjobSectiongenerateInvoice_Click(object sender, EventArgs e)
        {
            //job status must be ReadyForBilling, Collected or ReadyForPickup to generate an invoice
            if (_jobId <= 0) // Ensure _jobId is valid
            {
                MessageBox.Show("Invalid Job ID.", "Error");
                return;
            }
            if (cmbboxJobStatus.SelectedItem == null || (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem != Core.Enums.JobStatus.ReadyForBilling &&
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem != Core.Enums.JobStatus.Collected &&
                (Core.Enums.JobStatus)cmbboxJobStatus.SelectedItem != Core.Enums.JobStatus.ReadyForPickup)
            {
                MessageBox.Show("Job must be in 'Ready for Billing', 'Collected' or 'Ready for Pickup' status to generate an invoice.", "Error");
                return;
            }

            using (InvoiceDialog invoiceDialog = new InvoiceDialog(_serviceProvider, _jobId))
            {
                invoiceDialog.ShowDialog(this); // Show as a modal dialog
            }

        }


        public async void RefreshAndShowRepairTaskGrid()
        {
            await _repairTaskGridView.RefreshGridView(); // Tell the grid to refresh its data
                                                         // Then switch back to the grid view
            SwitchTaskView(panelrepairsectionTab, _repairTaskGridView);
        }


        public async void RefreshAndShowUpgradeTaskGrid()
        {
            await _upgradeTaskGridView.RefreshGridView(); // Tell the grid to refresh its data
            // Then switch back to the grid view
            SwitchTaskView(panelupgradeSectionTab, _upgradeTaskGridView);
        }

        public async void RefreshAndShowServiceTaskGrid()
        {
            await _serviceTaskGridView.RefreshGridView(); // Tell the grid to refresh its data
            // Then switch back to the grid view
            SwitchTaskView(panelsericetaskSectionTab, _serviceTaskGridView);
            
        }
    }
}
