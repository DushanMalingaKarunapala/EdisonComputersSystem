using BLL.Services.CustomerService;
using Core.IService;
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

namespace Presentation.SubSections.Customers
{
    public partial class EditCustomerSection : UserControl
    {
        private readonly IServiceProvider _serviceProvider; // Service provider for dependency injection
        private readonly int _customerId; //private field to hold the customer ID
        private readonly Action _oncompleteCallback; //callback to the customer grid view to refresh the data after editing a customer

        public EditCustomerSection(IServiceProvider serviceProvider, int customerId, Action oncompleteCallback)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            //_customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _customerId = customerId; //initialize the customer ID

            this.Load += EditCustomerSection_Load; // Subscribe to the Load event
            _oncompleteCallback = oncompleteCallback;
        }


        //load the customer details into the textboxes when the section is loaded
        private async void EditCustomerSection_Load(object sender, EventArgs e)
        {
            InitializeCustomerStatusComboBox();
            await LoadCustomer(_customerId); //load the customer details into the textboxes for editing
        }


        private void InitializeCustomerStatusComboBox()
        {
            // Initialize the customer status combo box with enum values
            cmbboxCusStatus.DataSource = Enum.GetValues(typeof(Core.Enums.RecordStatus));
            //prevent the user from typing in the combo box
            cmbboxCusStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }


        private async Task LoadCustomer(int customerId) {             //load the customer details into the textboxes for editing
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
                    var customer = await customerService.GetCustomerById(_customerId);
                    if (customer != null)
                    {
                        // Assuming you have textboxes to edit customer details

                        txtboxCusFirstname.Text = customer.FirstName;
                        txtboxCusLastName.Text = customer.LastName;
                        txtboxCusEmail.Text = customer.Email;
                        txtboxCusPhone.Text = customer.Phone;
                        txtboxCusAddress.Text = customer.Address;
                        datetimecusAddedDate.Value = customer.Created;
                        cmbboxCusStatus.SelectedItem = customer.RecordStatus; //set the selected item to the customer's record status
                    }
                    else
                    {
                        MessageBox.Show("Customer not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading customer data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }






        //update the customer details when the update button is clicked

        private async void btncusUpdate_Click(object sender, EventArgs e)
        {
            //update the customer details
            try
            {
                //set the UI to disabled while updating
                setUIEnabled(false);


                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();

                    var customer = new Core.Models.Customer
                    {
                        CustomerId = _customerId, //set the customer ID
                        FirstName = txtboxCusFirstname.Text,
                        LastName = txtboxCusLastName.Text,
                        Email = txtboxCusEmail.Text,
                        Phone = txtboxCusPhone.Text,
                        Address = txtboxCusAddress.Text,
                        Created = datetimecusAddedDate.Value, //use the date from the datetimepicker
                        Updated = DateTime.Now, //set the updated date to now
                        RecordStatus = (Core.Enums.RecordStatus)cmbboxCusStatus.SelectedItem //get the selected record status from the combo box





                    };
                    await customerService.UpdateCustomer(customer); //wait for the update to complete
                    MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //update the grid view with the new customer data
                    await customerService?.GetAllCustomers(); //refresh the customer service data

                    _oncompleteCallback?.Invoke(); //call the callback to refresh the customer grid view

                }



            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setUIEnabled(true); //re-enable the UI if there is an error
            }
            finally
            {
                // Re-enable the UI after the operation
                setUIEnabled(true);
            }
        }

        private void btneditcusSectionBack_Click(object sender, EventArgs e)
        {
            //call the callback to refresh the customer grid view
            _oncompleteCallback?.Invoke();
        }


        private void setUIEnabled(bool enabled)
        {
            //set the UI enabled or disabled
            txtboxCusFirstname.Enabled = enabled;
            txtboxCusLastName.Enabled = enabled;
            txtboxCusEmail.Enabled = enabled;
            txtboxCusPhone.Enabled = enabled;
            txtboxCusAddress.Enabled = enabled;
            datetimecusAddedDate.Enabled = enabled;
            btncusUpdate.Enabled = enabled;
            btneditcusSectionBack.Enabled = enabled;
        }


        private async void btneditcusSectionCancel_Click(object sender, EventArgs e)
        {
            //reload the customer again and reset the UI to its original state
            //call the load method to reload the customer details
            await LoadCustomer(_customerId); //reload the customer details into the textboxes
            




        }




    }
}
