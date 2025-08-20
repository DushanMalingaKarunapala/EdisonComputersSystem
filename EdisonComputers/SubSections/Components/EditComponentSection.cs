using BLL.Services.ComponentService;
using Core.Models;
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
    public partial class EditComponentSection : UserControl
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly Action _callback;
        private int _componentId; // Store the component ID for editing

        public EditComponentSection(IServiceProvider serviceProvider, Action callback, int componentId)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _callback = callback;
            _componentId = componentId; // Initialize the component ID

        }

        private async void EditComponentSection_Load(object sender, EventArgs e)
        {
            try
            {
                // Load the component types into the combo box
                editcomponentSectioncmbboxComponentType.DataSource = Enum.GetValues(typeof(Core.Enums.ComponentType)); // Assuming ComponentType is an enum
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the component types: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            await LoadComponentDetails(_componentId); // Replace 1 with the actual component ID you want to edit
        }

        //load the component details into the form fields for editing
        private async Task LoadComponentDetails(int componentId)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var componentService = scope.ServiceProvider.GetRequiredService<ComponentService>();
                    var component = await componentService.GetComponentById(componentId);
                    if (component != null)
                    {
                        editcomponentSectiontxtboxcomponentName.Text = component.Name;
                        editcomponentSectioncmbboxComponentType.SelectedItem = component.Type; // Assuming Type is an enum
                        editcomponentSectiontxtboxcomponentUnitPrice.Text = component.Price.ToString("F2"); // Format to 2 decimal places
                        editcomponentSectiontxtboxcomponentDescription.Text = component.Description;
                        editcomponentSectiontxtboxcomponentSerialNo.Text = component.SerialNumber;
                        editcomponentSectiontxtboxcomponentqty.Text = component.QuantityInStock.ToString();
                        editcomponentSectiontxtboxcomponentWarranty.Text = component.DefaultWarrantyInMonths?.ToString() ?? string.Empty; // Handle nullable warranty
                    }
                    else
                    {
                        MessageBox.Show("Component not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the component details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void editcomponentSectionbtnupdateComponent_Click(object sender, EventArgs e)
        {
            try
            {
                // Set the UI to disabled state while processing
                SetUIEnabledState(false);

                //validate inputs
                // --- 1. Validate Inputs ---
                if (string.IsNullOrWhiteSpace(editcomponentSectiontxtboxcomponentName.Text))
                {
                    MessageBox.Show("Component Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    
                    editcomponentSectiontxtboxcomponentName.Focus();
                    SetUIEnabledState(true); // Re-enable the UI
                    return;
                }
                if (editcomponentSectioncmbboxComponentType.SelectedItem == null)
                {
                    MessageBox.Show("Please select a component type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    editcomponentSectioncmbboxComponentType.Focus();
                    SetUIEnabledState(true); // Re-enable the UI
                    return;
                }
                //convert the price to decimal
                if (!decimal.TryParse(editcomponentSectiontxtboxcomponentWarranty.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    editcomponentSectiontxtboxcomponentUnitPrice.Focus();
                    SetUIEnabledState(true); // Re-enable the UI
                    return;
                }


                int? defaultWarrantyInMonths = null; // Start with null for no warranty
                if (!string.IsNullOrWhiteSpace(editcomponentSectiontxtboxcomponentWarranty.Text)) // Only parse if user entered something
                {
                    if (int.TryParse(editcomponentSectiontxtboxcomponentWarranty.Text, out int warrantyPeriod) && warrantyPeriod >= 0)
                    {
                        defaultWarrantyInMonths = warrantyPeriod; // Assign the parsed value (can be 0 or more)
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid, non-negative number for warranty period in months.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        editcomponentSectiontxtboxcomponentWarranty.Focus();
                        SetUIEnabledState(true); // Re-enable the UI
                        return;
                    }
                }


                // --- 2. Create Component Object ---
                var updatedComponent = new Core.Models.Component
                {
                    ComponentId = _componentId, // Use the component ID for updating
                    Name = editcomponentSectiontxtboxcomponentName.Text,
                    Type = (Core.Enums.ComponentType)editcomponentSectioncmbboxComponentType.SelectedItem, // Assuming Type is an enum
                    Price = decimal.Parse(editcomponentSectiontxtboxcomponentUnitPrice.Text),
                    Description = editcomponentSectiontxtboxcomponentDescription.Text,
                    SerialNumber = editcomponentSectiontxtboxcomponentSerialNo.Text,
                    QuantityInStock = int.Parse(editcomponentSectiontxtboxcomponentqty.Text),
                    //newly added property - to represent the stock status and availability of the component
                    StockStatus = Core.Enums.ComponentStockStatus.StockStatus.InStock, // Default to InStock, can be changed later
                    DefaultWarrantyInMonths = defaultWarrantyInMonths // Nullable type
                };

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var componentService = scope.ServiceProvider.GetRequiredService<ComponentService>();
                        // --- 3. Update Component ---
                        await componentService.UpdateComponent(updatedComponent);


                        MessageBox.Show("Component updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _callback?.Invoke(); // Call the callback to refresh the component list or perform any other action
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating the component: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetUIEnabledState(true); // Re-enable the UI if there was an error
                }
                finally
                {
                    // Re-enable the UI after the operation
                    SetUIEnabledState(true);
                }




            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void SetUIEnabledState(bool isEnabled)
        {
            // Enable or disable UI elements based on the isEnabled parameter
            editcomponentSectiontxtboxcomponentName.Enabled = isEnabled;
            editcomponentSectioncmbboxComponentType.Enabled = isEnabled;
            editcomponentSectiontxtboxcomponentUnitPrice.Enabled = isEnabled;
            editcomponentSectiontxtboxcomponentDescription.Enabled = isEnabled;
            editcomponentSectiontxtboxcomponentSerialNo.Enabled = isEnabled;
            editcomponentSectiontxtboxcomponentqty.Enabled = isEnabled;
            editcomponentSectiontxtboxcomponentWarranty.Enabled = isEnabled;
            editcomponentSectionbtnupdateComponent.Enabled = isEnabled;
        }



        private void editcomponentSectionbtncomponentSectionBack_Click(object sender, EventArgs e)
        {
            _callback?.Invoke(); // Call the callback to switch back to the component grid view or previous section
        }

        private async void editcomponentSectionbtncancel_Click(object sender, EventArgs e)
        {
            //reload the component again and reset the UI to its original state
            await LoadComponentDetails(_componentId); // Reload the component details to reset the UI

        }
    }
}
