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
                var validationErrors = new List<string>();

                if (string.IsNullOrWhiteSpace(editcomponentSectiontxtboxcomponentName.Text))
                    validationErrors.Add("Component Name is required.");

                if (editcomponentSectioncmbboxComponentType.SelectedItem == null)
                    validationErrors.Add("Please select a component type.");

                // Fix: Parse price from the correct textbox
                if (!decimal.TryParse(editcomponentSectiontxtboxcomponentUnitPrice.Text, out decimal price) || price < 0)
                    validationErrors.Add("Please enter a valid, non-negative price.");

                if (!int.TryParse(editcomponentSectiontxtboxcomponentqty.Text, out int quantity) || quantity < 0)
                    validationErrors.Add("Please enter a valid, non-negative quantity.");

                // Validate warranty
                int? warrantyPeriod = null;
                if (!string.IsNullOrWhiteSpace(editcomponentSectiontxtboxcomponentWarranty.Text))
                {
                    if (!int.TryParse(editcomponentSectiontxtboxcomponentWarranty.Text, out int period) || period < 0)
                    {
                        validationErrors.Add("Please enter a valid, non-negative warranty period.");
                    }
                    else
                    {
                        warrantyPeriod = period > 0 ? period : null;
                    }
                }

                if (validationErrors.Any())
                {
                    MessageBox.Show(string.Join("\n", validationErrors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SetUIEnabledState(true);
                    return;
                }


                // --- 2. Create Component Object ---
                var updatedComponent = new Core.Models.Component
                {
                    ComponentId = _componentId, // Use the component ID for updating
                    Name = editcomponentSectiontxtboxcomponentName.Text,
                    Type = (Core.Enums.ComponentType)editcomponentSectioncmbboxComponentType.SelectedItem, // Assuming Type is an enum
                    Price = price,
                    Description = editcomponentSectiontxtboxcomponentDescription.Text,
                    SerialNumber = editcomponentSectiontxtboxcomponentSerialNo.Text,
                    QuantityInStock = quantity,
                    //newly added property - to represent the stock status and availability of the component
                    StockStatus = Core.Enums.ComponentStockStatus.StockStatus.InStock, // Default to InStock, can be changed later
                    DefaultWarrantyInMonths = warrantyPeriod // Nullable type


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
