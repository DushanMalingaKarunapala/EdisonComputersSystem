using BLL.Services.RepairRecordService;
using Core.Enums;
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

namespace Presentation.SubSections.Tasks.RepairTask
{
    public partial class EditRepairTask : UserControl
    {
        private IServiceProvider serviceProvider;
        private int jobId;
        private int repairTaskId;
        //private int repairTaskId;
        private Action _callback;

        //to hold the components selected for this edit repair task session
        private List<RepairRecordComponentViewModel> _currentrepairComponents = new List<RepairRecordComponentViewModel>();

        private RepairRecord _loadedRepairRecord; // To hold the loaded repair record

        public EditRepairTask(IServiceProvider serviceProvider, int jobId, int repairTaskId, Action callback)
        {

            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.jobId = jobId;
            this.repairTaskId = repairTaskId;
            _callback = callback;

            cmbboxSelectWarrantyUnit.DataSource = Enum.GetValues(typeof(Core.Enums.WarrantyUnit)); //  WarrantyUnit is an enum
        }

        private void btneditRepairSectionBack_Click(object sender, EventArgs e)
        {
            //call back to the repair task grid view
            _callback?.Invoke();
        }

        private async void EditRepairTask_Load(object sender, EventArgs e)
        {
            await LoadRepairTask(repairTaskId);
            AddActionColumns(); // Ensure action columns are added after loading the task
            refreshSelectedComponentsGridView(); // Refresh the DataGridView to show any selected components
        }

        private async Task LoadRepairTask(int repairTaskId)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var repairRecordService = scope.ServiceProvider.GetRequiredService<RepairRecordService>();
                    _loadedRepairRecord = await repairRecordService.GetRepairRecordbyIdwithComponents(repairTaskId);
                    if (_loadedRepairRecord != null)
                    {
                        // Populate the form fields with the repair task data
                        txtboxaddrepairsectionDescription.Text = _loadedRepairRecord.ProblemDescription;
                        txtboxeditrepairSectionWarrantyPeriod.Text = _loadedRepairRecord.WarrantyPeriodinMonths.ToString();
                        txtboxaddrepairsectionRepairNotes.Text = _loadedRepairRecord.RepairNotes;
                        lbleditrepairrecordSectionID.Text = _loadedRepairRecord.RepairRecordId.ToString();
                        cmbboxaddRepairSectionRepairStatus.DataSource = Enum.GetValues(typeof(Core.Enums.RepairStatus));
                        cmbboxaddRepairSectionRepairStatus.SelectedItem = _loadedRepairRecord.RepairStatus; // Assuming RepairStatus is an enum
                        cmbboxSelectWarrantyUnit.SelectedItem = _loadedRepairRecord.WarrantyUnit; // Assuming WarrantyUnit is an enum


                        // Populate _currentrepairComponents from loaded data
                        _currentrepairComponents = _loadedRepairRecord.RepairRecordComponents
                            .Select(rrc => new RepairRecordComponentViewModel // Use your ViewModel
                            {
                                ComponentId = rrc.ComponentId,
                                ComponentName = rrc.Component?.Name ?? "N/A", // Ensure Component is included in GetRepairRecordbyIdwithComponents
                                QuantityUsed = rrc.QuantityUsed,
                                PricePerUnit = rrc.PricePerUnit
                            }).ToList();

                        refreshSelectedComponentsGridView(); // Display them


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading repair task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private async void btnrepairSectionUpdate_Click(object sender, EventArgs e)
        {
            //set the UI to disabled to prevent multiple clicks

            setuiEnabled(false);


            if (_loadedRepairRecord == null)
            {
                MessageBox.Show("No repair record loaded to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setuiEnabled(true); // Re-enable the UI elements
                return;
            }

            //validte the warranty period input
            int? warrantyPeriod = null; // Make it nullable
            WarrantyUnit warrantyUnit = (WarrantyUnit)cmbboxSelectWarrantyUnit.SelectedItem; // Get the selected unit

            // Validate the warranty period input
            if (!string.IsNullOrWhiteSpace(txtboxeditrepairSectionWarrantyPeriod.Text))
            {
                if (int.TryParse(txtboxeditrepairSectionWarrantyPeriod.Text, out int period) && period >= 0)
                {
                    //warrantyPeriod = period > 0 ? period : (int?)null; // Store null if 0 is entered, or just period if 0 is valid
                    // Only store a value if it's greater than 0. An empty box or 0 means no warranty.
                    if (period > 0)
                    {
                        warrantyPeriod = period;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid non-negative warranty period in months.", "Invalid Input");
                    txtboxeditrepairSectionWarrantyPeriod.Focus();
                    setuiEnabled(true); // Re-enable the UI elements
                    return;
                }
            }






            //create repair record object with scaler properties from the form
            var updatedRepairRecord = new RepairRecord
            {
                RepairRecordId = _loadedRepairRecord.RepairRecordId, // Use the loaded ID
                JobId = jobId,
                ProblemDescription = txtboxaddrepairsectionDescription.Text,
                RepairNotes = txtboxaddrepairsectionRepairNotes.Text,
                WarrantyPeriodinMonths = warrantyPeriod,
                WarrantyUnit = warrantyUnit, // Use the selected unit
                RepairStatus = (Core.Enums.RepairStatus)cmbboxaddRepairSectionRepairStatus.SelectedItem
            };


            //convert the view models back to repair record components enitties for saving

            var componentsToSave = _currentrepairComponents.Select(vm => new RepairRecordComponent
            {
                RepairRecordId = updatedRepairRecord.RepairRecordId, // Use the updated RepairRecord ID
                ComponentId = vm.ComponentId,
                QuantityUsed = vm.QuantityUsed,
                PricePerUnit = vm.PricePerUnit,
                



            }).ToList();


            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var repairRecordService = scope.ServiceProvider.GetRequiredService<RepairRecordService>();
                    // Update the repair record and its components
                    await repairRecordService.UpdateRepairRecordWithComponentAsync(updatedRepairRecord, componentsToSave, null);
                }
                MessageBox.Show("Repair task updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _callback?.Invoke(); // Call the callback to refresh the view

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating repair task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setuiEnabled(true); // Re-enable the UI elements
                return;
            }
            finally
            {
                setuiEnabled(true); // Re-enable the UI elements after operation
            }


        }





        private void btneditRepairTaskSectionaddComponents_Click(object sender, EventArgs e)
        {
            //pass the current list of componets so the dialog can pre select them
            //
            using (var selectDialog = new RepairComponentSelectionDialog(serviceProvider, _currentrepairComponents))
            {
                selectDialog.StartPosition = FormStartPosition.CenterParent; // Good for dialogs


                if (selectDialog.ShowDialog(this) == DialogResult.OK) // Show as dialog relative to this UserControl
                {
                    //User clicked OK, update the local list with the NEW selections from the dialog
                    _currentrepairComponents = new List<RepairRecordComponentViewModel>(selectDialog._selectedComponents); //get the updated list
                    refreshSelectedComponentsGridView(); // Update the UI
                }
            }

        }


        private void AddActionColumns()
        {
            // Ensure DataGridView is not auto-generating columns if you want full control
            gridvieweditrepairSectionaddedComponents.AutoGenerateColumns = false;
            gridvieweditrepairSectionaddedComponents.Columns.Clear(); // Clear any existing columns

            // --- Columns for displaying existing Component info (ReadOnly) ---
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentId",
                HeaderText = "ID",
                DataPropertyName = "ComponentId",
                ReadOnly = true,
                Visible = false // Usually hide ID from user
            };
            gridvieweditrepairSectionaddedComponents.Columns.Add(idColumn);

            DataGridViewTextBoxColumn NameColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentName",
                HeaderText = "Component Name",
                DataPropertyName = "ComponentName",
                ReadOnly = true,
                Visible = true // Usually hide ID from user
            };
            gridvieweditrepairSectionaddedComponents.Columns.Add(NameColumn);

            DataGridViewTextBoxColumn PricePerUnitColumn = new DataGridViewTextBoxColumn
            {
                Name = "PricePerUnit",
                HeaderText = "Price Per Unit",
                DataPropertyName = "PricePerUnit",
                ReadOnly = true,
                Visible = true // Usually hide ID from user
            };
            gridvieweditrepairSectionaddedComponents.Columns.Add(PricePerUnitColumn);


            DataGridViewTextBoxColumn QtyUsedColumn = new DataGridViewTextBoxColumn
            {
                Name = "QuantityUsed",
                HeaderText = "Qty Used",
                DataPropertyName = "QuantityUsed",
                ReadOnly = true,
                Visible = true // Usually hide ID from user
            };
            gridvieweditrepairSectionaddedComponents.Columns.Add(QtyUsedColumn);

            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn
            {
                Name = "Remove",
                HeaderText = "Remove",
                Image = Resource1.Waste, // 🗑️
                Width = 2,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            gridvieweditrepairSectionaddedComponents.Columns.Add(deleteColumn);
        }



        private void refreshSelectedComponentsGridView()
        {
            // Clear existing rows
            //gridvieweditrepairSectionaddedComponents.Rows.Clear();
            // Add each component to the DataGridView
            //bind the _currentrepairComponents to the DataGridView

            gridvieweditrepairSectionaddedComponents.DataSource = _currentrepairComponents.Select(c => new
            {
                c.ComponentId,
                c.ComponentName,
                c.PricePerUnit,
                QuantityUsed = c.QuantityUsed.ToString() // Ensure this is a string for display
            }).ToList();
        }

        private void gridvieweditrepairSectionaddedComponents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && gridvieweditrepairSectionaddedComponents.Columns[e.ColumnIndex].Name == "Remove") // Check for your remove button column
            {
                if (_currentrepairComponents.Count > e.RowIndex)
                {
                    _currentrepairComponents.RemoveAt(e.RowIndex);
                    refreshSelectedComponentsGridView();
                }
            }

        }


        private void setuiEnabled(bool enabled)
        {
            // Enable or disable UI elements based on the parameter
            btneditRepairSectionBack.Enabled = enabled;
            btnrepairSectionUpdate.Enabled = enabled;
            btneditRepairTaskSectionaddComponents.Enabled = enabled;
            txtboxaddrepairsectionDescription.Enabled = enabled;
            txtboxaddrepairsectionRepairNotes.Enabled = enabled;
            txtboxeditrepairSectionWarrantyPeriod.Enabled = enabled;
            cmbboxaddRepairSectionRepairStatus.Enabled = enabled;
            gridvieweditrepairSectionaddedComponents.Enabled = enabled;
        }

        private void btneditRepairSectionCancel_Click(object sender, EventArgs e)
        {
            //reload the repair task again and reset the UI to its original state
            if (_loadedRepairRecord != null)
            {
                // Reset the form fields to the original loaded values
                txtboxaddrepairsectionDescription.Text = _loadedRepairRecord.ProblemDescription;
                txtboxeditrepairSectionWarrantyPeriod.Text = _loadedRepairRecord.WarrantyPeriodinMonths?.ToString() ?? string.Empty;
                txtboxaddrepairsectionRepairNotes.Text = _loadedRepairRecord.RepairNotes;
                cmbboxaddRepairSectionRepairStatus.SelectedItem = _loadedRepairRecord.RepairStatus; // Assuming RepairStatus is an enum
                lbleditrepairrecordSectionID.Text = _loadedRepairRecord.RepairRecordId.ToString();
                // Reset the components list to the original loaded components
                _currentrepairComponents = _loadedRepairRecord.RepairRecordComponents
                    .Select(rrc => new RepairRecordComponentViewModel
                    {
                        ComponentId = rrc.ComponentId,
                        ComponentName = rrc.Component?.Name ?? "N/A",
                        QuantityUsed = rrc.QuantityUsed,
                        PricePerUnit = rrc.PricePerUnit
                    }).ToList();
                refreshSelectedComponentsGridView(); // Refresh the DataGridView to show the original components
                setuiEnabled(true); // Re-enable the UI elements
            }
            else
            {
                MessageBox.Show("No repair record loaded to reset.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //or can call the await load repair task method again to reset the UI but this is more efficient because it is not making a database call again. it using the already loaded data from the _loadedRepairRecord variable
        }
    }
}
