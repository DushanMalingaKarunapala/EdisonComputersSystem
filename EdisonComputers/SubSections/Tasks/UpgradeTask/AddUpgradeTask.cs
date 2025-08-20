using BLL.Services.UpgradeRecordService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.SubSections.Tasks.UpgradeTask
{
    public partial class AddUpgradeTask : UserControl
    {
        private readonly IServiceProvider serviceProvider;
        private int jobId;
        private Action _callback;


        public AddUpgradeTask(IServiceProvider serviceProvider, int jobId, Action callback)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.jobId = jobId;
            _callback = callback;
        }

        private void btnaddUpgradeSectionBack_Click(object sender, EventArgs e)
        {
            // Callback to the upgrade task grid view
            _callback?.Invoke();

        }

        private async void btnaddupgradeSectionSave_Click(object sender, EventArgs e)
        {
            try
            {
                setuiElementsState(false);



                using (var scope = serviceProvider.CreateScope())
                {
                    // Create an instance of the UpgradeRecordService
                    var upgradeRecordService = scope.ServiceProvider.GetRequiredService<UpgradeRecordService>();
                    // Create a new UpgradeRecord object and populate it with data from the form
                    var upgradeRecord = new Core.Models.UpgradeRecord
                    {
                        JobId = jobId,
                        UpgradeType = (Core.Enums.UpgradeType)cmbboxaddUpgradeSectionUpgradeStatus.SelectedItem,
                        UpgradeDescription = txtboxaddupgradesectionDescription.Text,
                        //UpgradeDate = DateTime.Now // or any other date you want to set
                    };
                    // Save the upgrade record using the service
                    await upgradeRecordService.AddUpgradeRecordAsync(upgradeRecord);
                    // Optionally, you can reset the form or show a success message
                    MessageBox.Show("Upgrade task saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //callback to the upgrade task grid view
                    _callback?.Invoke();
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., log the error or show a message to the user
                MessageBox.Show($"An error occurred while saving the upgrade task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setuiElementsState(true); // Re-enable UI elements in case of error
            }
            finally
            {
                // Optionally, you can reset the form or perform any cleanup here
                setuiElementsState(true); // Re-enable UI elements after the operation

            }

        }

        private void AddUpgradeTask_Load(object sender, EventArgs e)
        {
            cmbboxaddUpgradeSectionUpgradeStatus.DataSource = Enum.GetValues(typeof(Core.Enums.UpgradeType));
        }


        private  void ClearFormFields()
        {
            cmbboxaddUpgradeSectionUpgradeStatus.SelectedIndex = -1; // Reset the combo box selection
            txtboxaddupgradesectionDescription.Clear(); // Clear the description text box
        }



        private void setuiElementsState(bool enabled)
        {
            cmbboxaddUpgradeSectionUpgradeStatus.Enabled = enabled;
            txtboxaddupgradesectionDescription.Enabled = enabled;
            btnaddupgradeSectionSave.Enabled = enabled;
            btnaddUpgradeSectionBack.Enabled = enabled;
        }

        private void btnaddUpgraddeSectionCancel_Click(object sender, EventArgs e)
        {
            // Clear the form fields when the cancel button is clicked
            ClearFormFields();
            setuiElementsState(true); // Re-enable UI elements


        }
    }
}
