using BLL.Services.CustomerService;
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
    public partial class ViewCustomerSection : UserControl
    {
        
        private readonly int _customerId; //private field to hold the customer ID
        //call back to the customer grid view to refresh the data after viewing a customer
        private readonly Action _callback;
        private readonly IServiceProvider _serviceProvider; // Service provider for dependency injection



        public ViewCustomerSection(IServiceProvider serviceProvider, int customerId, Action callback)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _customerId = customerId; //initialize the customer ID
            this.Load += ViewCustomerSection_Load; // Subscribe to the Load event
            _callback = callback;
        }

        private async void ViewCustomerSection_Load(object sender, EventArgs e)
        {
            try
            {   using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
                    var customer = await customerService.GetCustomerById(_customerId);
                    if (customer != null)
                    {
                        // Assuming you have labels or textboxes to display customer details
                        lblcusId.Text = customer.CustomerId.ToString();
                        lblcusFirstName.Text = customer.FirstName;
                        lblcusEmail.Text = customer.Email;
                        lblcusPhoneNum.Text = customer.Phone;
                        lblcusaddedDate.Text = customer.Created.ToString("MM/dd/yyyy");
                        lblcusLastName.Text = customer.LastName;
                        lblcusAddress.Text = customer.Address;
                        // Add more fields as necessary
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

        private void lblcusLastName_Click(object sender, EventArgs e)
        {
            
        }

        private void btnbackcustomerView_Click(object sender, EventArgs e)
        {
            // Navigate back to the customer grid view section
            _callback?.Invoke(); // Call the callback to refresh the grid view
        }
    }
}