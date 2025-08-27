using BLL.Services.ComponentService;
using BLL.Services.DeviceService;
using Core.Models;
using Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Sections.Components;
using Presentation.Sections.Customers;
using Presentation.SubSections.Customers;
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
    public partial class ComponentGridView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ComponentSection _componentSection;


        public ComponentGridView(IServiceProvider serviceProvider, ComponentSection componentSection)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _componentSection = componentSection;

            // Wire up the Load event to set up the grid view
            this.gridviewComponents.DataBindingComplete += gridviewComponents_DataBindingComplete;
        }


        private async void ComponentGridView_Load(object sender, EventArgs e)
        {

            //set up the grid view columns
            SetupGridViewColumns();

            // Populate the filter categories
            PopulateFilterCategories();

            // Load the components when the user control is loaded
            //await LoadAllComponentsAsync();
            await LoadPriorityComponentsByDefaultAsync(); // Load priority components by default

        }

        private void PopulateFilterCategories()
        {
            var categories = new List<FilterCategory>
        {
             new FilterCategory { DisplayName = "Priority Components", Value = "Priority" }, // New default
            new FilterCategory { DisplayName = "Show All", Value = "All" },
            //new FilterCategory { DisplayName = "By Record Status", Value = "RecordStatus" },
            new FilterCategory { DisplayName = "By Component Type", Value = "ComponentType" }
        };

            cmbboxcomponentsectionfilterCategory.DataSource = categories;
            cmbboxcomponentsectionfilterCategory.DisplayMember = "DisplayName";
            cmbboxcomponentsectionfilterCategory.ValueMember = "Value";

            // Wire up the event handler for when the user changes the filter category
            cmbboxcomponentsectionfilterCategory.SelectedIndexChanged += cmbboxcomponentsectionfilterCategory_SelectedIndexChanged;
        }




        private async Task LoadPriorityComponentsByDefaultAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var componentService = scope.ServiceProvider.GetRequiredService<ComponentService>();
                    // Get low stock components + recently added components
                    var priorityComponents = await componentService.GetPriorityComponentsAsync();
                    BindComponentsToGrid(priorityComponents.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading priority components: {ex.Message}", "Error");
            }
        }








        // A better refresh method that reloads based on current filter
        public async Task RefreshGridView()
        {
            if (cmbboxcomponentsectionfilterCategory.SelectedValue != null)
            {
                string category = cmbboxcomponentsectionfilterCategory.SelectedValue.ToString();

                switch (category)
                {
                    case "RecentActive":
                        await LoadPriorityComponentsByDefaultAsync();
                        break;
                    case "All":
                        await LoadAllComponentsAsync();
                        break;
                    default:
                        // For other filters with values
                        if (cmbboxcomponentsectionfilterValue.SelectedValue != null)
                        {
                            cmbboxcomponentsectionfilterValue_SelectedIndexChanged(this, EventArgs.Empty);
                        }
                        else
                        {
                            // Fallback to default if no specific value is selected
                            await LoadPriorityComponentsByDefaultAsync();
                        }
                        break;
                }
            }
            else
            {
                // Fallback if no category is selected
                await LoadPriorityComponentsByDefaultAsync();
            }
        }



        // this method is commented out because it is not used anymore 
        //private async Task LoadComponents()
        //{
        //    try
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            var componentService = scope.ServiceProvider.GetRequiredService<BLL.Services.ComponentService.ComponentService>();
        //            var components = await componentService.GetAllComponentsAsync();
        //            gridviewComponents.DataSource = components.Select(c => new
        //            {
        //                c.ComponentId,
        //                c.Name,
        //                c.SerialNumber,
        //                c.Price,
        //                ComponentType = c.Type.ToString(),
        //                c.QuantityInStock

        //            }).ToList();

        //            AddActionColumns(); // Add action columns after setting the data source
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occurred while loading components: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //}


        //add action button to the grid view
        private void AddActionColumns()
        {
            if (gridviewComponents.Columns.Contains("View")) return; // prevent duplication

            //DataGridViewImageColumn viewColumn = new DataGridViewImageColumn
            //{
            //    Name = "View",
            //    HeaderText = "   View",

            //    Image = Resource1.Eye2, // 👁️
            //    Width = 2,
            //    ImageLayout = DataGridViewImageCellLayout.Zoom,




            //};

            DataGridViewImageColumn editColumn = new DataGridViewImageColumn
            {
                Name = "Edit",
                HeaderText = "Edit",
                Image = Resource1.Create3, // ✏️
                Width = 2,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };

            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn
            {
                Name = "Delete",
                HeaderText = "Delete",
                Image = Resource1.Remove, // 🗑️
                Width = 2,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };

            //gridviewComponents.Columns.Add(viewColumn);
            gridviewComponents.Columns.Add(editColumn);
            gridviewComponents.Columns.Add(deleteColumn);
        }


        private void SetupGridViewColumns()
        {
            // Call this once,  in the UserControl's constructor or Load event.
            // Ensure it only runs once.
            if (gridviewComponents.Columns.Count > 0) return;

            gridviewComponents.AutoGenerateColumns = false; // Turn off auto-generation
            gridviewComponents.Columns.Clear();

            // Define  data columns MANUALLY
            gridviewComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ComponentId",
                HeaderText = "Component Id",
                DataPropertyName = "ComponentId", // Must match property name in CustomerViewModel
                Visible = false // Often hide the ID
            });
            gridviewComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ComponentName",
                HeaderText = "Component Name",
                DataPropertyName = "ComponentName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            gridviewComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ComponentType",
                HeaderText = "Component Type",
                DataPropertyName = "ComponentType",
                Width = 200
            });
            gridviewComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SerialNumber",
                HeaderText = "Serial Number",
                DataPropertyName = "SerialNumber",
                Width = 200
            });

            gridviewComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 250
            });

            gridviewComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitPrice",
                HeaderText = "Unit Price",
                DataPropertyName = "UnitPrice",
                Width = 150
            });

            gridviewComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "QuantityInStock",
                HeaderText = "Qty In Stock",
                DataPropertyName = "QuantityInStock",
                Width = 150,

            });

            gridviewComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DefaultWarrantyInMonths",
                HeaderText = "Default Warranty (Months)",
                DataPropertyName = "DefaultWarrantyInMonths",
                Width = 150
            });

            // NOW add  action columns
            AddActionColumns();
        }











        //handle the cell click event to perform actions like view, edit, delete

        private void gridviewComponents_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Ignore header row
            // Ensure the clicked cell is one of the action columns
            string columnName = gridviewComponents.Columns[e.ColumnIndex].Name;

            var componentId = (int)gridviewComponents.Rows[e.RowIndex].Cells["ComponentId"].Value;


            switch (columnName)
            {
                //case "View":
                //    // Handle view action
                //    var viewUC = new ViewCustomerSection(_customerService, customerId, () => _customerSection.SwitchCustomerSection(this)); //call back function to switch back to the grid view as third parameter
                //    _customerSection.SwitchCustomerSection(viewUC);

                //    break;
                case "Edit":
                    // Handle edit action
                    var editCC = new EditComponentSection(_serviceProvider, _componentSection.RefreshAndShowGrid, componentId);
                    _componentSection.SwitchComponentSection(editCC);
                    break;
                case "Delete":
                    // Handle delete action
                    var confirmResult = MessageBox.Show("Are you sure you want to delete this component?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirmResult == DialogResult.Yes)
                    {
                        try
                        {
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var componentService = scope.ServiceProvider.GetRequiredService<ComponentService>();
                                componentService.CancelComponent(componentId);
                                MessageBox.Show("Component deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // Refresh the grid view after deletion
                                _componentSection.RefreshAndShowGrid();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting component: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        private async void cmbboxcomponentsectionfilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbboxcomponentsectionfilterCategory.SelectedValue == null) return;

            string selectedCategory = cmbboxcomponentsectionfilterCategory.SelectedValue.ToString();

            // Clear the second ComboBox before populating
            cmbboxcomponentsectionfilterValue.DataSource = null;
            cmbboxcomponentsectionfilterValue.Enabled = true;

            switch (selectedCategory)
            {
                //case "RecordStatus":
                //    cmbboxcomponentsectionfilterValue.DataSource = Enum.GetValues(typeof(Core.Enums.RecordStatus));
                //    break;

                case "ComponentType":
                    cmbboxcomponentsectionfilterValue.DataSource = Enum.GetValues(typeof(Core.Enums.ComponentType));
                    break;

                case "All":

                    cmbboxcomponentsectionfilterValue.Enabled = false; // Nothing to select for "All"
                    await LoadAllComponentsAsync(); // Reload all customers
                    break;

                case "Priority":
                default:
                    cmbboxcomponentsectionfilterValue.Enabled = false; // No selection needed for Priority
                    await LoadPriorityComponentsByDefaultAsync(); // Load priority components
                    break;


                
            }

        }

        private async void cmbboxcomponentsectionfilterValue_SelectedIndexChanged(object sender, EventArgs e)
        {

            // This event fires when the user selects a specific value 
            if (cmbboxcomponentsectionfilterCategory.SelectedItem == null || cmbboxcomponentsectionfilterValue.SelectedValue == null) return;

            string category = cmbboxcomponentsectionfilterCategory.SelectedValue.ToString();
            object value = cmbboxcomponentsectionfilterValue.SelectedItem;

            List<Core.Models.Component> filteredComponents = new List<Core.Models.Component>();

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var componentService = scope.ServiceProvider.GetRequiredService<ComponentService>();
                    switch (category)
                    {
                        case "RecordStatus":
                            //filteredComponents = await deviceService.GetDeviceByRecordStatus((Core.Enums.RecordStatus)value);
                            break;
                        case "ComponentType":
                            filteredComponents = await componentService.GetComponentsByType((Core.Enums.ComponentType)value);
                            break;
                    }
                }
                BindComponentsToGrid(filteredComponents);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filter: {ex.Message}", "Error");
            }

        }


        private async Task LoadAllComponentsAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var componentService = scope.ServiceProvider.GetRequiredService<ComponentService>();

                    //if searchboxcomponentSection is not empty, then fcall the search method to search for components
                    if (!string.IsNullOrWhiteSpace(searchboxcomponentSection.Text))
                    {
                        var searchTerm = searchboxcomponentSection.Text.Trim();
                        var searchResults = await componentService.SearchComponentsAsync(searchTerm);
                        BindComponentsToGrid(searchResults.ToList());
                        return; // Exit after search
                    }




                    var components = await componentService.GetAllComponentsAsync();
                    // Note: GetAllCustomers returns IEnumerable, so need ToList()
                    BindComponentsToGrid(components.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Devices: {ex.Message}", "Error");
            }
        }


        private void BindComponentsToGrid(List<Core.Models.Component> components)
        {
            //if the components quantity in stock is less than 5, then color the row red



            var componentViewModels = components.Select(c => new ComponentViewModel
            {




                ComponentId = c.ComponentId,
                ComponentName = c.Name,
                SerialNumber = c.SerialNumber,
                UnitPrice = c.Price,
                ComponentType = c.Type,
                //if the quantity of each component is less than 5 , then color the row red

                //`if (c.QuantityInStock < 5) gridviewComponents.Rows[components.IndexOf(c)].DefaultCellStyle.BackColor = Color.Red;

                QuantityInStock = c.QuantityInStock,
                Description = c.Description,
                DefaultWarrantyInMonths = c.DefaultWarrantyInMonths




            }).ToList();

            // Check if gridview needs to be cleared (can cause flicker)
            // A better approach is to set binding source's datasource

            gridviewComponents.DataSource = componentViewModels;

        }



        //an even handler to set back color of the row based on the quantity in stock
        private void gridviewComponents_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //loop through each row and check the quantity in stock
            foreach (DataGridViewRow row in gridviewComponents.Rows)
            {
                // make sure not in the new row placeholder
                //if (row.IsNewRow) continue; // Skip the new row placeholder

                // Get the underlying ViewModel object for this row
                if (row.DataBoundItem is ComponentViewModel vm)
                {
                    // Apply the rule
                    if (vm.QuantityInStock < 5)
                    {
                        // Set the entire row's default cell style back color
                        //set only the quanity in stock cell to red
                        row.Cells["QuantityInStock"].Style.BackColor = Color.FromArgb(255, 224, 224); // A light red

                        //row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224); // A light red
                        //row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                    else
                    {
                        // IMPORTANT: Reset the color for rows that DON'T meet the condition
                        // This prevents rows from staying red after their stock is updated.
                        row.DefaultCellStyle.BackColor = SystemColors.Window; // Default grid color
                        row.DefaultCellStyle.ForeColor = SystemColors.ControlText; // Default text color
                    }
                }

            }
        }

        private  void searchboxcomponentSection_TextChanged(object sender, EventArgs e)
        {
            // This event fires when the user types in the search box
            // Call the method to load components based on the search term
            _ = LoadAllComponentsAsync();

        }
    }
}
