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
    public partial class UpgradeComponentSelectionDialog : Form
    {
        private readonly IServiceProvider _serviceProvider;

        public List<UpgradeRecordComponentViewModel> _selectedUpgradeComponents { get; private set; } //to return the selected components
        private List<ComponentSelectionViewModel> _availableComponents; //to hold the available components for selection

        public UpgradeComponentSelectionDialog(IServiceProvider serviceProvider, List<UpgradeRecordComponentViewModel> alreadyselectedupgradeComponents)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _selectedUpgradeComponents = alreadyselectedupgradeComponents ?? new List<UpgradeRecordComponentViewModel>();
        }


        private async void UpgradeComponentSelectionDialog_Load(object sender, EventArgs e)
        {
            await LoadAvailableComponentsAsync(); // Load components when the dialog loads
            PopulateFilterComboBox(); // Populate the filter combo box

        }



        private void PopulateFilterComboBox()
        {
            // Get all ComponentType enum values
            var componentTypes = Enum.GetValues(typeof(Core.Enums.ComponentType)).Cast<Core.Enums.ComponentType>().ToList();

            // Create a list for the data source that includes a "Show All" option
            var filterOptions = new List<object> { "Show All" }; // Start with a string for "All"
            filterOptions.AddRange(componentTypes.Cast<object>()); // Add the enum values

            cmbboxrupgraderecordComponentFilterCategory.DataSource = filterOptions; // Set the data source
            cmbboxrupgraderecordComponentFilterCategory.SelectedIndex = 0; // Default to "Show All"

            // Wire up the event handler
            cmbboxrupgraderecordComponentFilterCategory.SelectedIndexChanged += cmbboxrupgraderecordComponentFilterCategory_SelectedIndexChanged;
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
                        IsSelected = _selectedUpgradeComponents.Any(suc => suc.ComponentId == c.ComponentId), // Pre-select if already in the list
                        QuantityToUse = _selectedUpgradeComponents.FirstOrDefault(suc => suc.ComponentId == c.ComponentId)?.QuantityUsed ?? 1,
                        PricePerUnitOverride = _selectedUpgradeComponents.FirstOrDefault(suc => suc.ComponentId == c.ComponentId)?.PricePerUnit ?? c.Price, // Default to component price
                        //default warranty period for the component, can be overridden later
                        WarrantyPeriodInMonths = c.DefaultWarrantyInMonths ?? 0

                    }).ToList();




                    //bind the data to the DataGridView
                    gridviewselectUpgradeComponents.DataSource = new BindingList<ComponentSelectionViewModel>(_availableComponents);

                    AddActionColumns(); // Add action columns after setting the data source

                }

            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log them or show a message to the user
                MessageBox.Show($"An error occurred while loading components: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        //add action columns for isSelected(checkbox), quantity to used(combobox), and price per unit override(textbox)
        private void AddActionColumns()
        {
            // Ensure DataGridView is not auto-generating columns if you want full control
            gridviewselectUpgradeComponents.AutoGenerateColumns = false;
            gridviewselectUpgradeComponents.Columns.Clear(); // Clear any existing columns

            // --- Columns for displaying existing Component info (ReadOnly) ---
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentId",
                HeaderText = "ID",
                DataPropertyName = "ComponentId",
                ReadOnly = true,
                Visible = false // Usually hide ID from user
            };
            gridviewselectUpgradeComponents.Columns.Add(idColumn);

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentName", // Different from DataPropertyName if needed
                HeaderText = "Component Name",
                DataPropertyName = "Name", // From ComponentSelectionViewModel
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            gridviewselectUpgradeComponents.Columns.Add(nameColumn);

            DataGridViewTextBoxColumn typeColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentType",
                HeaderText = "Type",
                DataPropertyName = "Type",
                ReadOnly = true,
                Width = 100
            };
            gridviewselectUpgradeComponents.Columns.Add(typeColumn);

            DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn
            {
                Name = "ComponentPrice",
                HeaderText = "Store Price",
                DataPropertyName = "Price",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }, // Currency format
                Width = 80
            };
            gridviewselectUpgradeComponents.Columns.Add(priceColumn);

            DataGridViewTextBoxColumn stockColumn = new DataGridViewTextBoxColumn
            {
                Name = "StockQuantity",
                HeaderText = "Stock",
                DataPropertyName = "QuantityInStock",
                ReadOnly = true,
                Width = 60
            };
            gridviewselectUpgradeComponents.Columns.Add(stockColumn);

            // --- Editable Action Columns ---
            DataGridViewCheckBoxColumn selectColumn = new DataGridViewCheckBoxColumn
            {
                Name = "IsSelectedCol", // Give it a unique name
                HeaderText = "Select",
                DataPropertyName = "IsSelected", // Binds to your ViewModel property
                Width = 60
            };
            gridviewselectUpgradeComponents.Columns.Add(selectColumn);

            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn // Use TextBox for now, validate on OK
            {
                Name = "QuantityToUseCol",
                HeaderText = "Qty to Use",
                DataPropertyName = "QuantityToUse", // Binds to your ViewModel property
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
                // You will parse this as int when 'OK' is clicked
            };
            gridviewselectUpgradeComponents.Columns.Add(quantityColumn);

            DataGridViewTextBoxColumn priceOverrideColumn = new DataGridViewTextBoxColumn // Use TextBox, validate on OK
            {
                Name = "PricePerUnitOverrideCol",
                HeaderText = "Unit Price Override",
                DataPropertyName = "PricePerUnitOverride", // Binds to your ViewModel property
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
                // You will parse this as decimal when 'OK' is clicked
            };
            gridviewselectUpgradeComponents.Columns.Add(priceOverrideColumn);

            //Warranty period override column
            DataGridViewTextBoxColumn warrantyColumn = new DataGridViewTextBoxColumn
            {
                Name = "WarrantyPeriodCol",
                HeaderText = "Warranty Period (Months)",
                DataPropertyName = "WarrantyPeriodInMonths", // Binds to your ViewModel property
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };
            gridviewselectUpgradeComponents.Columns.Add(warrantyColumn);


        }

        private void btnbackfromUpgradecomponentDialog_Click(object sender, EventArgs e)
        {
            //back to the edit upgrade task section
            this.DialogResult = DialogResult.Cancel; // Indicate cancellation
        }





        private void btnaddcomponentsforupgradeTask_Click(object sender, EventArgs e)
        { // Ensure any cell currently being edited commits its value to the data source.
            this.gridviewselectUpgradeComponents.EndEdit();

            _selectedUpgradeComponents.Clear();
            bool validationError = false;

            // Iterate through the master list of components, filtering for only the selected ones.
            // The properties on 'vmItem' have been updated by the DataGridView's data binding.
            foreach (var vmItem in _availableComponents.Where(c => c.IsSelected))
            {
                // --- Validate QuantityToUse ---
                if (vmItem.QuantityToUse <= 0)
                {
                    MessageBox.Show($"Please enter a valid positive quantity for '{vmItem.Name}'.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validationError = true;
                    break; // Stop on first error
                }

                // --- Validate PricePerUnitOverride ---
                if (vmItem.PricePerUnitOverride < 0)
                {
                    MessageBox.Show($"Price override for '{vmItem.Name}' cannot be negative.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validationError = true;
                    break;
                }

                // --- Validate Warranty Period ---
                // Since WarrantyPeriodInMonths is nullable on the ViewModel, we check for negative values if it has one.
                if (vmItem.WarrantyPeriodInMonths.HasValue && vmItem.WarrantyPeriodInMonths < 0)
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
                _selectedUpgradeComponents.Add(new UpgradeRecordComponentViewModel
                {
                    ComponentId = vmItem.ComponentId,
                    ComponentName = vmItem.Name,
                    QuantityUsed = vmItem.QuantityToUse,
                    PricePerUnit = vmItem.PricePerUnitOverride, // PricePerUnitOverride is now the authoritative value
                    WarrantyPeriodInMonths = vmItem.WarrantyPeriodInMonths // Pass the nullable int
                });
            }

            if (!validationError)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // If there was a validation error, do not close the dialog
                MessageBox.Show("Please correct the highlighted errors before proceeding.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void cmbboxrupgraderecordComponentFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbboxrupgraderecordComponentFilterCategory.SelectedItem == null || _availableComponents == null)
            {
                return;
            }

            // Check if "Show All" is selected
            if (cmbboxrupgraderecordComponentFilterCategory.SelectedItem is string filterString && filterString == "Show All")
            {
                BindComponentsToGrid(_availableComponents); // Bind the full list
                return;
            }

            // If an enum value is selected
            if (cmbboxrupgraderecordComponentFilterCategory.SelectedItem is Core.Enums.ComponentType selectedType)
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
            gridviewselectUpgradeComponents.DataSource = new BindingList<ComponentSelectionViewModel>(componentsToDisplay);
        }

        //cancel the selected components upgrade
        private void btnCancelSelectedComponentsUpgrade_Click(object sender, EventArgs e)
        {
            // Clear the selected components and close the dialog with Cancel result
            _selectedUpgradeComponents.Clear();
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }
    }
}

