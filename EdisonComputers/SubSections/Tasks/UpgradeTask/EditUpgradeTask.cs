using BLL.Services.RepairRecordService;
using BLL.Services.UpgradeRecordService;
using Core.Models;
using Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Presentation.SubSections.Components;
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
    public partial class EditUpgradeTask : UserControl
    {
        private readonly IServiceProvider serviceProvider;
        private int jobId;
        private int upgradeTaskId;
        private Action callback;


        //to hold the components selected for this edit repair task session
        private List<UpgradeRecordComponentViewModel> _currentupgradeComponents = new List<UpgradeRecordComponentViewModel>();

        private UpgradeRecord _loadedUpgradeRecord; // To hold the loaded upgrade record

        public EditUpgradeTask(IServiceProvider serviceProvider, int jobId, int upgradeTaskId, Action callback)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.jobId = jobId;
            this.upgradeTaskId = upgradeTaskId;
            this.callback = callback;
        }

        private void btneditUpgradeSectionBack_Click(object sender, EventArgs e)
        {
            callback?.Invoke();
        }


        private async void EditUpgradeTask_Load(object sender, EventArgs e)
        {
            await LoadUpgradeTask(upgradeTaskId);
            AddActionColumns(); // Add action columns to the DataGridView
            refreshSelectedComponentsGridView(); // Refresh the DataGridView to show any existing components
        }





        private async Task LoadUpgradeTask(int upgradeTaskId)
        {

            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var upgradeRecordService = scope.ServiceProvider.GetRequiredService<UpgradeRecordService>();
                    _loadedUpgradeRecord = await upgradeRecordService.GetUpgradeRecordbyIdwithComponents(upgradeTaskId);
                    if (_loadedUpgradeRecord != null)
                    {
                        // Populate the form fields with the repair task data
                        txtboxeditupgradesectionDescription.Text = _loadedUpgradeRecord.UpgradeDescription;
                        //txtboxeditUpgradeSectionWarrantyPeriod.Text = _loadedUpgradeRecord.WarrantyExpiry.ToString();
                        lbleditupgradeSectionUpradeRecordID.Text = _loadedUpgradeRecord.UpgradeRecordId.ToString();
                        cmbboxeditUpgradeSectionUpgradeStatus.DataSource = Enum.GetValues(typeof(Core.Enums.UpgradeStatus));
                        cmbboxeditsectionUpradeType.DataSource = Enum.GetValues(typeof(Core.Enums.UpgradeType)); // Assuming UpgradeType is an enum
                        cmbboxeditUpgradeSectionUpgradeStatus.SelectedItem = _loadedUpgradeRecord.UpgradeStatus; // Assuming RepairStatus is an enum
                        cmbboxeditsectionUpradeType.SelectedItem = _loadedUpgradeRecord.UpgradeType; // Assuming UpgradeType is an enum


                        //  Set the Job navigation property for warranty calculation
                        if (_loadedUpgradeRecord.UpgradeRecordComponents != null)
                        {
                            foreach (var component in _loadedUpgradeRecord.UpgradeRecordComponents)
                            {
                                component.UpgradeRecord = _loadedUpgradeRecord; // Set the parent relationship
                            }
                        }



                        // Populate _currentupgradeComponents from loaded data
                        _currentupgradeComponents = _loadedUpgradeRecord.UpgradeRecordComponents
                            .Select(urc => new UpgradeRecordComponentViewModel // Use your ViewModel
                            {
                                ComponentId = urc.ComponentId,
                                ComponentName = urc.Component?.Name ?? "N/A", // Ensure Component is included in GetRepairRecordbyIdwithComponents
                                QuantityUsed = urc.QuantityUsed,
                                PricePerUnit = urc.PricePerUnit,
                                WarrantyPeriodInMonths = urc.WarrantyPeriodInMonths, // Use the warranty period from the entity
                                WarrantyExpiryDate = urc.WarrantyExpiryDate?.ToString("MM/dd/yyyy") ?? "No Warranty" // Assuming you want to display it as a string
                            }).ToList() ?? new List<UpgradeRecordComponentViewModel>(); ;

                        refreshSelectedComponentsGridView(); // Display them


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading repair task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        //method to refresh the components grid view
        private void refreshSelectedComponentsGridView()
        {
            // Clear existing rows
            //gridvieweditrepairSectionaddedComponents.Rows.Clear();
            // Add each component to the DataGridView
            //bind the _currentrepairComponents to the DataGridView

            gridvieweditUpgradeSectionaddedComponents.DataSource = _currentupgradeComponents.Select(c => new
            {
                c.ComponentId,
                c.ComponentName,
                c.PricePerUnit,
                WarrantyPeriodInMonths = c.WarrantyPeriodInMonths?.ToString() ?? "No Warranty", 

                c.WarrantyExpiryDate, // Display warranty period in months
                QuantityUsed = c.QuantityUsed.ToString() // Ensure this is a string for display

            }).ToList();
        }



        private void AddActionColumns()
        {
            // Ensure DataGridView is not auto-generating columns if you want full control
            gridvieweditUpgradeSectionaddedComponents.AutoGenerateColumns = false;
            gridvieweditUpgradeSectionaddedComponents.Columns.Clear(); // Clear any existing columns

            // --- Columns for displaying existing Component info (ReadOnly) ---
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentId",
                HeaderText = "ID",
                DataPropertyName = "ComponentId",
                ReadOnly = true,
                Visible = false // Usually hide ID from user
            };
            gridvieweditUpgradeSectionaddedComponents.Columns.Add(idColumn);

            DataGridViewTextBoxColumn NameColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentName",
                HeaderText = "Component Name",
                DataPropertyName = "ComponentName",
                ReadOnly = true,
                Visible = true // Usually hide ID from user
            };
            gridvieweditUpgradeSectionaddedComponents.Columns.Add(NameColumn);

            DataGridViewTextBoxColumn PricePerUnitColumn = new DataGridViewTextBoxColumn
            {
                Name = "PricePerUnit",
                HeaderText = "Price Per Unit",
                DataPropertyName = "PricePerUnit",
                ReadOnly = true,
                Visible = true // Usually hide ID from user
            };
            gridvieweditUpgradeSectionaddedComponents.Columns.Add(PricePerUnitColumn);


            DataGridViewTextBoxColumn QtyUsedColumn = new DataGridViewTextBoxColumn
            {
                Name = "QuantityUsed",
                HeaderText = "Qty Used",
                DataPropertyName = "QuantityUsed",
                ReadOnly = true,
                Visible = true // Usually hide ID from user
            };
            gridvieweditUpgradeSectionaddedComponents.Columns.Add(QtyUsedColumn);

            //column for warranty period in months
            DataGridViewTextBoxColumn warrantyColumn = new DataGridViewTextBoxColumn
            {
                Name = "WarrantyPeriodInMonths",
                HeaderText = "Warranty Period (Months)",
                DataPropertyName = "WarrantyPeriodInMonths",
                ReadOnly = true,
                Visible = true // Usually hide ID from user
            };
            gridvieweditUpgradeSectionaddedComponents.Columns.Add(warrantyColumn);


            // ✅ Warranty Expiry Date
            gridvieweditUpgradeSectionaddedComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WarrantyExpiryDate",
                HeaderText = "Warranty Expiry",
                DataPropertyName = "WarrantyExpiryDate",
                ReadOnly = true,
                Visible = true
            });




            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn
            {
                Name = "Remove",
                HeaderText = "Remove",
                Image = Resource1.Waste, // 🗑️
                Width = 2,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            gridvieweditUpgradeSectionaddedComponents.Columns.Add(deleteColumn);



        }

        private void btneditUpgradeTaskSectionaddComponents_Click(object sender, EventArgs e)
        {

            using (var selectDialog = new UpgradeComponentSelectionDialog(serviceProvider, _currentupgradeComponents))
            {
                selectDialog.StartPosition = FormStartPosition.CenterParent; // Good for dialogs


                if (selectDialog.ShowDialog(this) == DialogResult.OK) // Show as dialog relative to this UserControl
                {
                    //User clicked OK, update the local list with the NEW selections from the dialog
                    _currentupgradeComponents = new List<UpgradeRecordComponentViewModel>(selectDialog._selectedUpgradeComponents); //get the updated list
                    refreshSelectedComponentsGridView(); // Update the UI
                }
            }
        }

        private async void btnupgradeSectionUpdate_Click(object sender, EventArgs e)
        {

            //set the UI elements to disabled while updating
            setUIElementsEnabled(false);

            if (_loadedUpgradeRecord == null)
            {
                MessageBox.Show("No repair record loaded to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setUIElementsEnabled(true);
                return;
            }

            ////validte the warranty period input
            //DateTime? warrantyExpiryDate = null; // Make it nullable
            //if (!string.IsNullOrWhiteSpace(txtboxeditUpgradeSectionWarrantyPeriod.Text))
            //{
            //    if (int.TryParse(txtboxeditUpgradeSectionWarrantyPeriod.Text, out int warrantyPeriod))
            //    {
            //        if (warrantyPeriod > 0)
            //        {
            //            warrantyExpiryDate = DateTime.Now.AddMonths(warrantyPeriod);
            //        }
            //        else if (warrantyPeriod == 0) { /* No warranty */ }
            //        else
            //        {
            //            MessageBox.Show("Warranty period must be a non-negative number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            setUIElementsEnabled(true);
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please enter a valid numeric warranty period in months.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        setUIElementsEnabled(true);
            //        return;
            //    }
            //}






            //create upgrade record object with scaler properties from the form
            var updatedUpgradeRecord = new UpgradeRecord
            {
                UpgradeRecordId = _loadedUpgradeRecord.UpgradeRecordId, // Use the loaded ID
                JobId = jobId,
                UpgradeDescription = txtboxeditupgradesectionDescription.Text,
                //warranty expiry was removed because warranty is now calculated for each component used in the upgrade record
                //WarrantyExpiry = (DateTime)warrantyExpiryDate,
                UpgradeStatus = (Core.Enums.UpgradeStatus)cmbboxeditUpgradeSectionUpgradeStatus.SelectedItem,
                UpgradeType = (Core.Enums.UpgradeType)cmbboxeditsectionUpradeType.SelectedItem
            };


            //convert the view models back to upgrade record components enitties for saving

            var componentsToSave = _currentupgradeComponents.Select(vm => new UpgradeRecordComponent
            {
                UpgradeRecordId = updatedUpgradeRecord.UpgradeRecordId, // Use the updated Upgrade Record ID
                ComponentId = vm.ComponentId,
                QuantityUsed = vm.QuantityUsed,
                PricePerUnit = vm.PricePerUnit,
                WarrantyPeriodInMonths = vm.WarrantyPeriodInMonths // Use the warranty period from the ViewModel



            }).ToList();


            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var upgradeRecordService = scope.ServiceProvider.GetRequiredService<UpgradeRecordService>();
                    // Update the repair record and its components
                    await upgradeRecordService.UpdateUpgradeRecordWithComponentAsync(updatedUpgradeRecord, componentsToSave, null);
                }
                MessageBox.Show("Upgrade task updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                callback?.Invoke(); // Call the callback to refresh the view

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating Upgrade task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setUIElementsEnabled(true); // Re-enable UI elements on error
                return;
            }
            finally
            {
                setUIElementsEnabled(true); // Re-enable UI elements after operation
            }
        }


        private void gridvieweditUpgradeSectionaddedComponents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && gridvieweditUpgradeSectionaddedComponents.Columns[e.ColumnIndex].Name == "Remove")
            {

                if (_currentupgradeComponents.Count > e.RowIndex)
                {
                    _currentupgradeComponents.RemoveAt(e.RowIndex);
                    refreshSelectedComponentsGridView();
                }
            }
        }


        private void setUIElementsEnabled(bool enabled)
        {
            // Enable or disable all relevant UI elements
            txtboxeditupgradesectionDescription.Enabled = enabled;
            cmbboxeditUpgradeSectionUpgradeStatus.Enabled = enabled;
            cmbboxeditsectionUpradeType.Enabled = enabled;
            txtboxeditUpgradeSectionWarrantyPeriod.Enabled = enabled;
            btneditUpgradeTaskSectionaddComponents.Enabled = enabled;
            btnupgradeSectionUpdate.Enabled = enabled;
        }



        private async void btneditUpgradeSectionCancel_Click(object sender, EventArgs e)
        {
            //reload the upgrade task to reset any changes made and revert to the original state
            if (_loadedUpgradeRecord != null)
            {
                // Reset the form fields to the original loaded values
                await LoadUpgradeTask(_loadedUpgradeRecord.UpgradeRecordId);




            }
            else
            {
                MessageBox.Show("No upgrade task loaded to cancel changes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }



    }
}
