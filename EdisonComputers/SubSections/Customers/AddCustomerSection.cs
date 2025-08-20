using BLL.Services.CustomerService;
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

namespace Presentation.SubSections.Customers
{
    public partial class AddCustomerSection : UserControl
    {
        //inject iservice provider short lived service provider to resolve the customer service
        private readonly IServiceProvider _serviceProvider;
        //callback to the customer grid view to refresh the data after adding a new customer
        private readonly Action _callback;

        public AddCustomerSection(IServiceProvider serviceProvider, Action callback)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            _callback = callback;
        }

        private async void btncusSave_Click(object sender, EventArgs e)
        {
            try
            {
                //set the UI to disabled to prevent multiple clicks
                SetUIEnabled(false);

                //validate the customer data before saving
                if (!ValidateCustomerData())
                {
                    SetUIEnabled(true); // Re-enable UI if validation fails
                    return;
                }




                var customer = new Customer
            {
                LastName = txtboxCusLastName.Text,
                FirstName = txtboxCusFirstname.Text,

                Email = txtboxCusEmail.Text,
                Phone = txtboxCusPhone.Text,
                Address = txtboxCusAddress.Text,
                Created = DateTime.Now,


            };


                // Create a scope JUST for this save operation
                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
                    await customerService.AddCustomer(customer);
                }


                //_customerService.GetAllCustomers();
                MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                _callback?.Invoke(); // Call the callback to refresh the grid view
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetUIEnabled(true); // Re-enable UI after operation
            }
        }

        private void btnaddcusSectionBack_Click(object sender, EventArgs e)
        {
            // Navigate back to the customer grid view section
            _callback?.Invoke(); // Call the callback to refresh the grid view

        }

        private void ClearFields()
        {
            txtboxCusLastName.Clear();
            txtboxCusFirstname.Clear();
            txtboxCusEmail.Clear();
            txtboxCusPhone.Clear();
            txtboxCusAddress.Clear();
        }

        private void btnaddcusSectionCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
        }


        //method to validate the customer data before saving
        private bool ValidateCustomerData()
        {
            if (string.IsNullOrWhiteSpace(txtboxCusFirstname.Text) || string.IsNullOrWhiteSpace(txtboxCusPhone.Text))
            {
                MessageBox.Show("First Name and Phone are required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtboxCusEmail.Text) && !txtboxCusEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        //method to set ui enabled or disabled based on the validation result

        private void SetUIEnabled(bool isEnabled)
        {
            txtboxCusLastName.Enabled = isEnabled;
            txtboxCusFirstname.Enabled = isEnabled;
            txtboxCusEmail.Enabled = isEnabled;
            txtboxCusPhone.Enabled = isEnabled;
            txtboxCusAddress.Enabled = isEnabled;
            btncusSave.Enabled = isEnabled;
            btnaddcusSectionBack.Enabled = isEnabled;
            btnaddcusSectionCancel.Enabled = isEnabled;
        }
    }


    
    }
