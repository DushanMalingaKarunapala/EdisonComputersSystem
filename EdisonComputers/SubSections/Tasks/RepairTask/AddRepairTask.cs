using BLL.Services.RepairRecordService;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Presentation.SubSections.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.SubSections.Tasks.RepairTask
{
    public partial class AddRepairTask : UserControl
    {
        private IServiceProvider serviceProvider;

        private int jobId;
        private Action _callback;
        // a list to hold the using components for the repair task
        private List<RepairRecordComponent> _repairRecordComponents = new List<RepairRecordComponent>();

        public AddRepairTask(IServiceProvider serviceProvider, int jobId, Action callback)
        {
            InitializeComponent();

            this.serviceProvider = serviceProvider;
            this.jobId = jobId;


            _callback = callback;
        }

        private void btnaddRepairSectionBack_Click(object sender, EventArgs e)
        {
            //callback to the the repair task grid view
            _callback?.Invoke();
        }

        private async void btnaddrepairSectionSave_Click(object sender, EventArgs e)
        {
            //add the repair task for this job 

            //btnaddComponentsRepairSection.Enabled = false; // Disable the button to prevent multiple clicks

            try
            {


                // set the UI to disabled to prevent multiple clicks
                SetUIEnabled(false);

                //if the jobId is not valid, show an error message and return
                if (jobId <= 0)
                {
                    MessageBox.Show("Invalid Job ID. Please select a valid job.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetUIEnabled(true); // Re-enable the UI elements
                    return;
                }
                // Check if the job exists in the database
                using (var scope = serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<BLL.Services.JobService.JobService>();
                    var job = await jobService.GetJobById(jobId);
                    if (job == null)
                    {
                        MessageBox.Show("Job not found. Please select a valid job.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SetUIEnabled(true); // Re-enable the UI elements
                        return;
                    }
                }

               

                using (var scope = serviceProvider.CreateScope())
                {

                    //calculate the warranty expiry date based on the provided warranty period in months
                    // Assuming txtboxaddrepairSectionWarrantyPeriod is a TextBox for inputting warranty period in months

                    //validte the warranty period input
                    //validte the warranty period input
                    int? warrantyPeriod = null; // Make it nullable
                    if (!string.IsNullOrWhiteSpace(txtboxaddrepairSectionWarrantyPeriod.Text))
                    {
                        if (int.TryParse(txtboxaddrepairSectionWarrantyPeriod.Text, out int period) && period >= 0)
                        {
                            warrantyPeriod = period > 0 ? period : (int?)null; // Store null if 0 is entered, or just period if 0 is valid
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid non-negative warranty period in months.", "Invalid Input");
                            txtboxaddrepairSectionWarrantyPeriod.Focus();
                            SetUIEnabled(true); // Re-enable the UI elements
                            return;
                        }
                    }




                    var repairService = scope.ServiceProvider.GetRequiredService<RepairRecordService>();
                    // Create a new RepairRecord instance
                    var repairRecord = new Core.Models.RepairRecord
                    {
                        JobId = jobId,
                        ProblemDescription = txtboxaddrepairsectionDescription.Text,
                        RepairNotes = txtboxaddrepairsectionRepairNotes.Text,
                        RepairStatus = (Core.Enums.RepairStatus)cmbboxaddRepairSectionRepairStatus.SelectedItem,

                        WarrantyPeriodinMonths = warrantyPeriod
                    };

                    // Save the repair record using the service
                    await repairService.AddRepairRecordAsync(repairRecord);

                    MessageBox.Show("Repair task added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Clear the form fields after saving
                    txtboxaddrepairsectionDescription.Clear();
                    txtboxaddrepairsectionRepairNotes.Clear();
                    cmbboxaddRepairSectionRepairStatus.SelectedIndex = 0;
                    txtboxaddrepairSectionWarrantyPeriod.Clear();
                    // Callback to refresh the view
                    _callback?.Invoke();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding repair task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                // Re-enable the UI elements after the operation
                SetUIEnabled(true);

            }
        }

        private void AddRepairTask_Load(object sender, EventArgs e)
        {
            cmbboxaddRepairSectionRepairStatus.DataSource = Enum.GetValues(typeof(Core.Enums.RepairStatus));
        }


        private void SetUIEnabled(bool enabled)
        {
            // Enable or disable UI elements as needed
            btnaddrepairSectionSave.Enabled = enabled;
            btnaddRepairSectionBack.Enabled = enabled;
            txtboxaddrepairsectionDescription.Enabled = enabled;
            txtboxaddrepairsectionRepairNotes.Enabled = enabled;
            cmbboxaddRepairSectionRepairStatus.Enabled = enabled;
            txtboxaddrepairSectionWarrantyPeriod.Enabled = enabled;
        }

        private void ClearFields()
        {
            // Clear the input fields
            txtboxaddrepairsectionDescription.Clear();
            txtboxaddrepairsectionRepairNotes.Clear();
            cmbboxaddRepairSectionRepairStatus.SelectedIndex = 0; // Reset to the first item
            txtboxaddrepairSectionWarrantyPeriod.Clear(); // Clear warranty period input
        }

        private void btnaddRepairSectionCancel_Click(object sender, EventArgs e)
        {
            // Clear the fields and reset the UI
            ClearFields();
            SetUIEnabled(true);
            

        }
    }
}
