using BLL.Services.ComponentService;
using Core.ViewModels;
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

namespace Presentation.SubSections.Components
{
    public partial class RepairComponentSelectionDialog : Form
    {
        private readonly IServiceProvider _serviceProvider;

        public List<RepairRecordComponentViewModel> _selectedComponents { get; private set; } //to return the selected components
        private List<ComponentSelectionViewModel> _availableComponents; //to hold the available components for selection

        //this class represents a dialog for selecting components for a specific repair task or upgrade task.

        public RepairComponentSelectionDialog(IServiceProvider serviceProvider, List<RepairRecordComponentViewModel> alreadyselectedComponents)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _selectedComponents = alreadyselectedComponents ?? new List<RepairRecordComponentViewModel>();



        }


        private void PopulateFilterComboBox()
        {
            // Get all ComponentType enum values
            var componentTypes = Enum.GetValues(typeof(Core.Enums.ComponentType)).Cast<Core.Enums.ComponentType>().ToList();

            // Create a list for the data source that includes a "Show All" option
            var filterOptions = new List<object> { "Show All" }; // Start with a string for "All"
            filterOptions.AddRange(componentTypes.Cast<object>()); // Add the enum values

            cmbboxrepairrecorComponentFilterCategory.DataSource = filterOptions; // Set the data source
            cmbboxrepairrecorComponentFilterCategory.SelectedIndex = 0; // Default to "Show All"

            // Wire up the event handler
            cmbboxrepairrecorComponentFilterCategory.SelectedIndexChanged += cmbboxrepairrecorComponentFilterCategory_SelectedIndexChanged;
        }






        private async Task LoadAvailableComponentsAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var componentService = scope.ServiceProvider.GetRequiredService<ComponentService>();

                    var components = await componentService.GetOnlyAvailableComponentsAsync();

                    _availableComponents = components.Select(c => new ComponentSelectionViewModel
                    {
                        ComponentId = c.ComponentId,
                        Name = c.Name,
                        Type = c.Type.ToString(), // Assuming Type is an enum, convert to string
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        IsSelected = _selectedComponents.Any(sc => sc.ComponentId == c.ComponentId), // Pre-select if already in the list
                        QuantityToUse = _selectedComponents.FirstOrDefault(sc => sc.ComponentId == c.ComponentId)?.QuantityUsed ?? 1,
                        PricePerUnitOverride = _selectedComponents.FirstOrDefault(sc => sc.ComponentId == c.ComponentId)?.PricePerUnit ?? c.Price, // Default to component price
                        //warranty period for specific component in the upgrade
                        WarrantyPeriodInMonths = c.DefaultWarrantyInMonths ?? 0// Assuming this is part of the component model
                    }).ToList();

                    //bind the data to the DataGridView
                    gridviewselectComponents.DataSource = new BindingList<ComponentSelectionViewModel>(_availableComponents);

                    AddActionColumns(); // Add action columns after setting the data source

                }

            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log them or show a message to the user
                MessageBox.Show($"An error occurred while loading components: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ComponentSelectionDialog_Load(object sender, EventArgs e)
        {
            // Load ALL components first
            await LoadAvailableComponentsAsync();
            // Then populate the filter ComboBox
            PopulateFilterComboBox();
        }

        //add action columns for isSelected(checkbox), quantity to used(combobox), and price per unit override(textbox)
        private void AddActionColumns()
        {
            // Ensure DataGridView is not auto-generating columns if you want full control
            gridviewselectComponents.AutoGenerateColumns = false;
            gridviewselectComponents.Columns.Clear(); // Clear any existing columns

            // --- Columns for displaying existing Component info (ReadOnly) ---
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentId",
                HeaderText = "ID",
                DataPropertyName = "ComponentId",
                ReadOnly = true,
                Visible = false // Usually hide ID from user
            };
            gridviewselectComponents.Columns.Add(idColumn);

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentName", // Different from DataPropertyName if needed
                HeaderText = "Component Name",
                DataPropertyName = "Name", // From ComponentSelectionViewModel
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            gridviewselectComponents.Columns.Add(nameColumn);

            DataGridViewTextBoxColumn typeColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentType",
                HeaderText = "Type",
                DataPropertyName = "Type",
                ReadOnly = true,
                Width = 100
            };
            gridviewselectComponents.Columns.Add(typeColumn);

            DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentPrice",
                HeaderText = "Store Price",
                DataPropertyName = "Price",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }, // Currency format
                Width = 80
            };
            gridviewselectComponents.Columns.Add(priceColumn);

            DataGridViewTextBoxColumn stockColumn = new DataGridViewTextBoxColumn
            {
                Name = "StockQuantity",
                HeaderText = "Stock",
                DataPropertyName = "QuantityInStock",
                ReadOnly = true,
                Width = 60
            };
            gridviewselectComponents.Columns.Add(stockColumn);

            // --- Editable Action Columns ---
            DataGridViewCheckBoxColumn selectColumn = new DataGridViewCheckBoxColumn
            {
                Name = "IsSelectedCol", // Give it a unique name
                HeaderText = "Select",
                DataPropertyName = "IsSelected", // Binds to  ViewModel property
                Width = 60
            };
            gridviewselectComponents.Columns.Add(selectColumn);

            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn // Use TextBox for now, validate on OK
            {
                Name = "QuantityToUseCol",
                HeaderText = "Qty to Use",
                DataPropertyName = "QuantityToUse", // Binds to  ViewModel property
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
                // You will parse this as int when 'OK' is clicked
            };
            gridviewselectComponents.Columns.Add(quantityColumn);

            DataGridViewTextBoxColumn priceOverrideColumn = new DataGridViewTextBoxColumn // Use TextBox, validate on OK
            {
                Name = "PricePerUnitOverrideCol",
                HeaderText = "Unit Price Override",
                DataPropertyName = "PricePerUnitOverride", // Binds to  ViewModel property
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
                // You will parse this as decimal when 'OK' is clicked
            };
            gridviewselectComponents.Columns.Add(priceOverrideColumn);

            //warranty period overde column
            DataGridViewTextBoxColumn warrantyColumn = new DataGridViewTextBoxColumn
            {
                Name = "WarrantyPeriodCol",
                HeaderText = "Warranty (Months)",
                DataPropertyName = "WarrantyPeriodInMonths", // Binds to  ViewModel property
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };
            gridviewselectComponents.Columns.Add(warrantyColumn);
        }


        //modify button click event to return the selected components
        private void btnaddcomponentsforrepairTask_Click(object sender, EventArgs e)
        {

            // Ensure any pending grid edits are committed to the data source
            this.gridviewselectComponents.EndEdit();

            _selectedComponents.Clear();
            bool validationError = false;

            // --- CORRECTED LOOP ---
            // Iterate through the master list of available components,
            // filtering for only the ones the user has selected via the checkbox.
            // The IsSelected, QuantityToUse, PricePerUnitOverride, and WarrantyPeriodInMonths
            // properties of vmItem have been updated by the DataGridView's data binding.
            foreach (var vmItem in _availableComponents.Where(c => c.IsSelected))
            {
                // --- Validate QuantityToUse ---
                if (vmItem.QuantityToUse <= 0)
                {
                    MessageBox.Show($"Please enter a valid positive quantity for '{vmItem.Name}'.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // We can't easily focus the cell here, so this general message is the best we can do without more complex code.
                    validationError = true;
                    break;
                }

                // --- Validate PricePerUnitOverride ---
                if (vmItem.PricePerUnitOverride < 0)
                {
                    MessageBox.Show($"Price override for '{vmItem.Name}' cannot be negative.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validationError = true;
                    break;
                }

                // --- Validate Warranty Period ---
                if (vmItem.WarrantyPeriodInMonths < 0)
                {
                    MessageBox.Show($"Warranty period for '{vmItem.Name}' cannot be negative.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validationError = true;
                    break;
                }

                // --- Stock Check ---
                if (vmItem.QuantityToUse > vmItem.QuantityInStock)
                {
                    MessageBox.Show($"Cannot use {vmItem.QuantityToUse} of {vmItem.Name}. Only {vmItem.QuantityInStock} in stock.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validationError = true;
                    break;
                }

                // If all validations pass for this item, add it to the results list.
                _selectedComponents.Add(new RepairRecordComponentViewModel
                {
                    ComponentId = vmItem.ComponentId,
                    ComponentName = vmItem.Name,
                    QuantityUsed = vmItem.QuantityToUse,
                    PricePerUnit = vmItem.PricePerUnitOverride, // Use the value from the VM, it's already defaulted if empty
                                                                // For a Repair Task, there's no per-component warranty, so we don't need to add it here.
                                                                // For the Upgrade dialog, you would add:
                                                                // WarrantyPeriodInMonths = vmItem.WarrantyPeriodInMonths
                });
            }



            if (!validationError)
            {
                // If we reach here, all validations passed
                this.DialogResult = DialogResult.OK; // Set dialog result to OK
                this.Close(); // Close the dialog

            }
            // If validationError is true, the dialog stays open for the user to correct.
        }

        private void btnbackfromcomponentDialog_Click(object sender, EventArgs e)
        {
            //back to the edit repair task section
            this.DialogResult = DialogResult.Cancel; // Set dialog result to Cancel
        }








        // Event handler for the filter by type ComboBox
        private void cmbboxrepairrecorComponentFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbboxrepairrecorComponentFilterCategory.SelectedItem == null || _availableComponents == null)
            {
                return;
            }

            // Check if "Show All" is selected
            if (cmbboxrepairrecorComponentFilterCategory.SelectedItem is string filterString && filterString == "Show All")
            {
                BindComponentsToGrid(_availableComponents); // Bind the full list
                return;
            }

            // If an enum value is selected
            if (cmbboxrepairrecorComponentFilterCategory.SelectedItem is Core.Enums.ComponentType selectedType)
            {
                // Filter the master list of available components
                var filteredList = _availableComponents
                    .Where(c => c.Type == selectedType.ToString())
                    .ToList();

                BindComponentsToGrid(filteredList); // Bind the filtered list
            }





        }


        // NEW HELPER METHOD: To bind a list to the grid
        private void BindComponentsToGrid(List<ComponentSelectionViewModel> componentsToDisplay)
        {
            // Use BindingList for two-way data binding, which is good for DataGridView
            gridviewselectComponents.DataSource = new BindingList<ComponentSelectionViewModel>(componentsToDisplay);
        }


        //cancel selected components button click event
        private void btnCancelSelectedComponents_Click(object sender, EventArgs e)
        {
            //Clear the selected components and close the dialog with Cancel result
            _selectedComponents.Clear();
            //this.DialogResult = DialogResult.Cancel; // Set dialog result to Cancel
            //this.Close(); // Close the dialog

        }


        //private void cmbboxrepairrecordComponentFilterValue_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}
    }




}
