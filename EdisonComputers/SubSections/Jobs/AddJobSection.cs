using BLL.Services.CustomerService;
using BLL.Services.DeviceService;
using BLL.Services.JobService;
using Core.IRepository;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.SubSections.Jobs
{
    public partial class AddJobSection : UserControl
    {
        private readonly IServiceProvider _serviceProvider; // service provider to resolve dependencies
        private readonly Action _callback; // callback to return to the previous section

        public AddJobSection(IServiceProvider serviceProvider, Action callback)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));

            //call the Load event
            this.Load += AddJobSection_Load;

            //use selectionchangecomitted for user driven selection changes to the customer combobox
            //this.cmbboxaddjobSectionSelectCustomer1.SelectionChangeCommitted += cmbboxaddjobSectionSelectCustomer1_SelectionChangeCommitted;

            //use validating event for user driven selection changes to the customer combobox
            this.cmbboxaddjobSectionSelectCustomer1.Validating += cmbboxaddjobSectionSelectCustomer1_Validating;

        }



        private void btnaddjobsectionBack_Click(object sender, EventArgs e)
        {
            //invoke the callback to switch back to the previous section
            _callback?.Invoke();
        }





        private async void AddJobSection_Load(object sender, EventArgs e)
        {


            // 1. First, tell the control to allow typing.
            cmbboxaddjobSectionSelectCustomer1.DropDownStyle = ComboBoxStyle.DropDown;

            // 2. Next, tell it WHERE its suggestion list comes from.
            cmbboxaddjobSectionSelectCustomer1.AutoCompleteSource = AutoCompleteSource.ListItems;

            // 3. FINALLY, tell it HOW to perform the autocompletion.
            cmbboxaddjobSectionSelectCustomer1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;


            // Load customers asynchronously when the section loads
            await LoadCustomersAsnc();

            if (cmbboxaddjobSectionSelectCustomer1.Items.Count > 0 && cmbboxaddjobSectionSelectCustomer1.SelectedItem != null)
            {
                // After customers are loaded, if there's a selected customer, load their devices
                if (cmbboxaddjobSectionSelectCustomer1.SelectedItem is Customer selectedCustomer)
                {
                    await LoadDevicesByCustomerIdAsync(selectedCustomer.CustomerId); //removed configure await (false) from here to avoid deadlocks in UI thread
                }
                else if (cmbboxaddjobSectionSelectCustomer1.SelectedValue is int customerId) // Fallback if the selected item is not a Customer object
                {
                    await LoadDevicesByCustomerIdAsync(customerId);//removed configure await (false) from here  to avoid deadlocks in UI thread
                }


            }
            else
            {
                //no customers found, clear the device combobox and set a message
                cmbboxaddjobSectionSelectDevice.DataSource = null; // Clear previous data source
            }



        }

        //method to populate the device combobox based on the selected customer
        //this method was removed 
        //private async void cmbboxaddjobSectionSelectCustomer_SelectedIndexChanged_1(object sender, EventArgs e)
        //{

        //    if (cmbboxaddjobSectionSelectCustomer.SelectedItem is Core.Models.Customer selectedCustomer)
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
        //            var devices = await deviceService.GetDevicebyCustomerId(selectedCustomer.CustomerId);



        //            cmbboxaddjobSectionSelectDevice.DataSource = devices;
        //            cmbboxaddjobSectionSelectDevice.DisplayMember = "Model";
        //            cmbboxaddjobSectionSelectDevice.ValueMember = "DeviceId";
        //        }
        //    }
        //}






        //event handler for the Add New Job button click
        private async void btnaddnewJob_Click(object sender, EventArgs e)
        {


            //set the UI to disabled to prevent multiple clicks
            EnableUIControls(false);

            //validate the combobox selections

            //if the selected customer is in deleted status, prevent adding a job
            if (cmbboxaddjobSectionSelectCustomer1.SelectedItem is Customer selectedCustomer && selectedCustomer.RecordStatus == Core.Enums.RecordStatus.Deleted)
            {
                MessageBox.Show("Cannot add a job for a deleted customer. Please select an active customer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                EnableUIControls(true); // Re-enable the UI controls
                return;
            }



            if (cmbboxaddjobSectionSelectCustomer1.SelectedItem == null || cmbboxaddjobSectionSelectDevice.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer and a device before adding a job.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //get the selected ids
            var customerId = cmbboxaddjobSectionSelectCustomer1.SelectedValue as int?;
            var deviceId = cmbboxaddjobSectionSelectDevice.SelectedValue as int?;
            if (customerId == null || deviceId == null)
            {
                MessageBox.Show("Please select a valid customer and device.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jobDescription = addjobSectionJobDescription.Text.Trim();
            DateTime checkedinDate = DateTime.Now; // Use current date and time for checked-in date
            Decimal manualJobCost = 0; // Default to 0 if not provided

            var newJob = new Job
            {

                DeviceId = deviceId.Value,
                JobDescription = jobDescription,
                CheckedInDate = checkedinDate,
                ManualJobCost = manualJobCost,
                JobStatus = Core.Enums.JobStatus.CheckedIn // Default job status
            };

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
                    await jobService.AddJob(customerId.Value, deviceId.Value, newJob); //pass the customer if for validation in the service layer

                    MessageBox.Show("Job added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Optionally, clear the form fields after adding the job
                    addjobSectionJobDescription.Clear();
                    cmbboxaddjobSectionSelectCustomer1.SelectedIndex = -1;
                    cmbboxaddjobSectionSelectDevice.SelectedIndex = -1;

                    //load the viewjob section to show the newly added job 
                    _callback?.Invoke(); // Call the callback to refresh the grid view or switch sections



                }



            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(argEx.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (KeyNotFoundException knfEx)
            {
                MessageBox.Show(knfEx.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException invOpEx)
            {
                MessageBox.Show($"An error occurred while adding the job: {invOpEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the job: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
            finally
            {
                // Re-enable the UI controls after the operation
                EnableUIControls(true);
            }


        }


        //method to load customers to the customer combobox asynchronously

        private async Task LoadCustomersAsnc()
        {
            try
            {

                //populate the customer combobox  

                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
                    var customers = await customerService.GetAllCustomers();

                    cmbboxaddjobSectionSelectCustomer1.DataSource = null; // Clear previous data source

                    cmbboxaddjobSectionSelectCustomer1.DataSource = customers;
                    cmbboxaddjobSectionSelectCustomer1.DisplayMember = "FullName";
                    cmbboxaddjobSectionSelectCustomer1.ValueMember = "CustomerId";


                    if (customers == null)
                    {
                        cmbboxaddjobSectionSelectCustomer1.Text = "No customers found";
                        cmbboxaddjobSectionSelectDevice.DataSource = null;
                        cmbboxaddjobSectionSelectDevice.Text = "Select a customer first";
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading customers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //method to load the devices to the device combobox asynchronously based on the selected customer

        private async Task LoadDevicesByCustomerIdAsync(int customerId)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
                    var devices = await deviceService.GetDevicebyCustomerId(customerId);
                    cmbboxaddjobSectionSelectDevice.DataSource = devices;
                    cmbboxaddjobSectionSelectDevice.DisplayMember = "Model";
                    cmbboxaddjobSectionSelectDevice.ValueMember = "DeviceId";

                    if (devices == null || devices.Count == 0)
                    {
                        cmbboxaddjobSectionSelectDevice.DataSource = null; // Clear previous data source
                        cmbboxaddjobSectionSelectDevice.Text = "No devices found for this customer";
                    }
                    else
                    {
                        cmbboxaddjobSectionSelectDevice.Text = "Select a device"; // Reset text when devices are available
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading devices: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbboxaddjobSectionSelectCustomer1.DataSource = null; // Clear customer selection on error
            }
        }


        //event handler for when the customer selection changes
        //this method was removed in favor of the Validating event handler
        //private async void cmbboxaddjobSectionSelectCustomer1_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    if (cmbboxaddjobSectionSelectCustomer1.SelectedItem is Customer selectedCustomer)
        //    {
        //        // Load devices for the selected customer
        //        await LoadDevicesByCustomerIdAsync(selectedCustomer.CustomerId).ConfigureAwait(false);
        //    }
        //    else if (cmbboxaddjobSectionSelectCustomer1.SelectedValue is int customerId) //fallback
        //    {
        //        // Load devices for the selected customer by ID
        //        await LoadDevicesByCustomerIdAsync(customerId).ConfigureAwait(false);
        //    }
        //    else
        //    {
        //        cmbboxaddjobSectionSelectDevice.DataSource = null; // Clear device selection if no customer is selected
        //        cmbboxaddjobSectionSelectDevice.Text = "Select a customer first";
        //    }
        //}


        // Validating event handler for the customer combobox. this will correctly handle user input and selection changes
        private async void cmbboxaddjobSectionSelectCustomer1_Validating(object sender, CancelEventArgs e)
        {
            // Check if the selected item is a valid Customer object
            if (cmbboxaddjobSectionSelectCustomer1.SelectedItem is Customer selectedCustomer)
            {
                // Load devices for the selected customer
                await LoadDevicesByCustomerIdAsync(selectedCustomer.CustomerId);
            }
            else if (cmbboxaddjobSectionSelectCustomer1.SelectedValue is int customerId) // Fallback for edge cases
            {
                await LoadDevicesByCustomerIdAsync(customerId);
            }
            else
            {
                // The text in the box doesn't match any customer in the list.
                // You can choose to clear the device list or even show an error.
                cmbboxaddjobSectionSelectDevice.DataSource = null;
                cmbboxaddjobSectionSelectDevice.Text = "Invalid customer selected";
                // Optionally, you could set e.Cancel = true to prevent the user from leaving the control
                // and show a warning message, but for now, just clearing the next box is fine.
            }
        }


        private void EnableUIControls(bool enable)
        {
            // Enable or disable UI controls based on the parameter
            cmbboxaddjobSectionSelectCustomer1.Enabled = enable;
            cmbboxaddjobSectionSelectDevice.Enabled = enable;
            addjobSectionJobDescription.Enabled = enable;
            btnaddnewJob.Enabled = enable;
        }
    }
}
