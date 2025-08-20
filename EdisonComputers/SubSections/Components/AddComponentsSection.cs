using BLL.Services.ComponentService;
using Core.Enums;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Presentation.SubSections
{
    public partial class AddComponentsSection : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Action _callback;

        public AddComponentsSection(IServiceProvider serviceProvider, Action callback)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _callback = callback;
        }

        private async void AddComponentsSection_Load(object sender, EventArgs e)
        {
            try
            {
                // Load any necessary data or perform initialization here
                cmbboxComponentType.DataSource = Enum.GetValues(typeof(ComponentType)); // Assuming ComponentType is an enum
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the add components section: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }












        private async void btnsaveComponent_Click(object sender, EventArgs e)
        {
            try
            {

                //set the UI to disabled to prevent multiple clicks
                setuiEnabled(false);

                //validate inputs
                // --- 1. Validate Inputs ---
                if (string.IsNullOrWhiteSpace(txtboxcomponentName.Text))
                {
                    MessageBox.Show("Component Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtboxcomponentName.Focus();
                    setuiEnabled(true);
                    return;

                }
                if (cmbboxComponentType.SelectedItem == null)
                {
                    MessageBox.Show("Please select a component type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbboxComponentType.Focus();
                    setuiEnabled(true);
                    return;
                }
                //convert the price to decimal
                if (!decimal.TryParse(txtboxcomponentUnitPrice.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    setuiEnabled(true);
                    return;
                }


                int? defaultWarrantyInMonths = null; // Start with null for no warranty
                if (!string.IsNullOrWhiteSpace(txtboxcomponentWarranty.Text)) // Only parse if user entered something
                {
                    if (int.TryParse(txtboxcomponentWarranty.Text, out int warrantyPeriod) && warrantyPeriod >= 0)
                    {
                        defaultWarrantyInMonths = warrantyPeriod; // Assign the parsed value (can be 0 or more)
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid, non-negative number for warranty period in months.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtboxcomponentWarranty.Focus();
                        setuiEnabled(true);
                        return;
                    }
                }




                decimal nudcomponentPrice = price; // Assuming nudcomponentPrice is a NumericUpDown control for price
                //check if the quantity is a positive integer 
                if (!int.TryParse(txtboxcomponentqty.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Please enter a valid quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    setuiEnabled(true);
                    return;
                }


                //set ui enabled if validation fails



                // Create a new component object
                var component = new Core.Models.Component
                {
                    SerialNumber = txtboxcomponentSerialNo.Text.Trim(),
                    Name = txtboxcomponentName.Text.Trim(),
                    Type = (ComponentType)cmbboxComponentType.SelectedItem, // Assuming ComponentType is an enum
                    Description = txtboxcomponentDescription.Text.Trim(),
                    Price = nudcomponentPrice, // Assuming nudcomponentPrice is a NumericUpDown control for price
                    DefaultWarrantyInMonths = defaultWarrantyInMonths,
                    StockStatus = ComponentStockStatus.StockStatus.InStock, // Default stock status
                    QuantityInStock = quantity // Assuming txtboxcomponentqty is a TextBox for quantity
                };

                // Add the component using the service provider
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var componentService = scope.ServiceProvider.GetRequiredService<ComponentService>();
                        await componentService.AddComponentAsync(component);

                        //message
                        MessageBox.Show("Component added succsssfully!");

                        _callback?.Invoke(); // Call the callback to switch back to the component grid view section

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while saving the component: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the component: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Re-enable the UI after the operation
                setuiEnabled(true);
                ClearFields(); // Clear the fields after saving
            }
        }

        private void btncomponentSectionBack_Click(object sender, EventArgs e)
        {
            //call the callback to switch back to the component grid view section

            _callback?.Invoke();
        }








        private void ClearFields()
        {
            txtboxcomponentName.Clear();
            txtboxcomponentSerialNo.Clear();
            cmbboxComponentType.SelectedIndex = -1; // Reset the combo box selection
            txtboxcomponentDescription.Clear();
            txtboxcomponentUnitPrice.Clear();
            txtboxcomponentWarranty.Clear();
            txtboxcomponentqty.Clear();
        }

        private void setuiEnabled(bool enabled)
        {
            txtboxcomponentName.Enabled = enabled;
            txtboxcomponentSerialNo.Enabled = enabled;
            cmbboxComponentType.Enabled = enabled;
            txtboxcomponentDescription.Enabled = enabled;
            txtboxcomponentUnitPrice.Enabled = enabled;
            txtboxcomponentWarranty.Enabled = enabled;
            txtboxcomponentqty.Enabled = enabled;
            btnsaveComponent.Enabled = enabled;
            btncomponentSectionBack.Enabled = enabled;
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            // Clear all fields and reset the UI
            ClearFields();
            setuiEnabled(true);
           

        }
    }
}
