using BLL.Services.CustomerService;
using Core.Models;
using Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Properties;
using Presentation.Sections.Customers;
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

    public partial class CustomerGridView : UserControl
    {

        private readonly CustomerSection _customerSection;
        private readonly IServiceProvider _serviceProvider; // Store the service provider
        private readonly Action _callback; // Callback to refresh and switch back to grid view

        public CustomerGridView(CustomerSection customerSection, IServiceProvider serviceProvider, Action callback)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _customerSection = customerSection;
            _callback = callback; // Store the callback for later use


        }

        private async void CustomerGridView_Load(object sender, EventArgs e)
        {
            SetupGridViewColumns(); // Define the grid structure first

            PopulateFilterCategories(); // Populate the first ComboBox
            //await LoadAllCustomersAsync(); // Load all data initially
            await LoadRecentActiveCustomersByDefaultAsync(); // Load recent active customers by default


        }

        private void PopulateFilterCategories()
        {
            var categories = new List<FilterCategory>
        {
            new FilterCategory { DisplayName = "Active Customers (6 Months)", Value = "RecentActive" },
            new FilterCategory { DisplayName = "Show All", Value = "All" },
            new FilterCategory { DisplayName = "By Record Status", Value = "RecordStatus" },
            new FilterCategory { DisplayName = "By Device Type", Value = "DeviceType" }
        };

            cmbboxcustomerFilterCategory.DataSource = categories;
            cmbboxcustomerFilterCategory.DisplayMember = "DisplayName";
            cmbboxcustomerFilterCategory.ValueMember = "Value";

            // Wire up the event handler for when the user changes the filter category
            cmbboxcustomerFilterCategory.SelectedIndexChanged += cmbboxcustomerFilterCategory_SelectedIndexChanged;
        }






        private async Task LoadRecentActiveCustomersByDefaultAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
                    // Get customers active in last 6 months (have recent jobs or added recently)
                    var recentCustomers = await customerService.GetRecentActiveCustomersAsync(6);
                    BindCustomersToGrid(recentCustomers.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading recent customers: {ex.Message}", "Error");
            }
        }











        //method to refresh the grid view data
        // A better refresh method that reloads based on current filter
        public async Task RefreshGridView()
        {
            //if (cmbboxcustomerFilterCategory.SelectedValue?.ToString() == "All" || cmbboxcustomerFilterValue.SelectedItem == null)
            //{
            //    await LoadAllCustomersAsync();
            //}
            //else
            //{
            //    // Re-apply the current filter
            //    cmbboxcustomerFilterValue_SelectedIndexChanged(this, EventArgs.Empty);
            //}
            //new approach to handle the refresh based on selected category and value
            if (cmbboxcustomerFilterCategory.SelectedValue != null)
            {
                string category = cmbboxcustomerFilterCategory.SelectedValue.ToString();

                switch (category)
                {
                    case "RecentActive":
                        await LoadRecentActiveCustomersByDefaultAsync();
                        break;
                    case "All":
                        await LoadAllCustomersAsync();
                        break;
                    default:
                        // For other filters with values
                        if (cmbboxcustomerFilterValue.SelectedValue != null)
                        {
                            cmbboxcustomerFilterValue_SelectedIndexChanged(this, EventArgs.Empty);
                        }
                        else
                        {
                            // Fallback to default if no specific value is selected
                            await LoadRecentActiveCustomersByDefaultAsync();
                        }
                        break;
                }
            }
            else
            {
                // Fallback if no category is selected
                await LoadRecentActiveCustomersByDefaultAsync();
            }
        }





        //add action button to the grid view
        private void AddActionColumns()
        {
            if (gridviewCustomer.Columns.Contains("View")) return; // prevent duplication

            DataGridViewImageColumn viewColumn = new DataGridViewImageColumn
            {
                Name = "View",
                HeaderText = "   View",

                Image = Resource1.Eye3, // 👁️
              

               
                Width = 2,
                ImageLayout = DataGridViewImageCellLayout.Zoom,




            };

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
            //if the record is deleted or cancelled, disable the delete button

            //deleteColumn.DefaultCellStyle.NullValue = Resource1.Culture; // Set the default image for delete column


            gridviewCustomer.Columns.Add(viewColumn);
            gridviewCustomer.Columns.Add(editColumn);
            gridviewCustomer.Columns.Add(deleteColumn);
        }

        private void SetupGridViewColumns()
        {
            // Call this once,  in the UserControl's constructor or Load event.
            // Ensure it only runs once.
            if (gridviewCustomer.Columns.Count > 0) return;

            gridviewCustomer.AutoGenerateColumns = false; // Turn off auto-generation
            gridviewCustomer.Columns.Clear();

            // Define  data columns MANUALLY
            gridviewCustomer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Customer ID",
                DataPropertyName = "Id", // Must match property name in CustomerViewModel
                Visible = false // Often hide the ID
            });
            gridviewCustomer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                HeaderText = "Full Name",
                DataPropertyName = "FullName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            gridviewCustomer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email Address",
                DataPropertyName = "Email",
                Width = 200
            });
            gridviewCustomer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone",
                HeaderText = "Phone Number",
                DataPropertyName = "Phone",
                Width = 120
            });

            gridviewCustomer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Address",
                HeaderText = "Address",
                DataPropertyName = "Address",
                Width = 200
            });

            gridviewCustomer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Added",
                HeaderText = "Added Date",
                DataPropertyName = "Added",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy/MM/dd h:mm tt", FormatProvider = System.Globalization.CultureInfo.InvariantCulture } // Format date
            });
            gridviewCustomer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 200
            });

            // NOW add  action columns
            AddActionColumns();
        }









        // Handle cell click events to perform actions based on the clicked cell
        private async void gidviewCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Ignore header row
            // Ensure the clicked cell is one of the action columns
            string columnName = gridviewCustomer.Columns[e.ColumnIndex].Name;

            var customerId = (int)gridviewCustomer.Rows[e.RowIndex].Cells["Id"].Value;


            switch (columnName)
            {
                case "View":
                    // Handle view action
                    var viewUC = new ViewCustomerSection(_serviceProvider, customerId, () => _customerSection.SwitchCustomerSection(this)); //call back function to switch back to the grid view as third parameter
                    _customerSection.SwitchCustomerSection(viewUC);

                    break;
                case "Edit":
                    // Handle edit action
                    var editUC = new EditCustomerSection(_serviceProvider, customerId, _customerSection.RefreshAndShowGrid);
                    _customerSection.SwitchCustomerSection(editUC);
                    break;
                case "Delete":
                    // Handle delete action
                    //cancel the customer record status to deleted when the delete button is clicked
                    var confirmResult = MessageBox.Show("Are you sure you want to Cancel this customer?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirmResult == DialogResult.Yes)
                    {
                        try
                        {
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
                                await customerService.CancelCustomer(customerId);

                                MessageBox.Show("Customer Cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _callback?.Invoke(); // Call the callback to refresh the grid view


                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void cmbboxcustomerFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbboxcustomerFilterCategory.SelectedValue == null) return;

            string selectedCategory = cmbboxcustomerFilterCategory.SelectedValue.ToString();

            // Clear the second ComboBox before populating
            cmbboxcustomerFilterValue.DataSource = null;
            cmbboxcustomerFilterValue.Enabled = true;

            switch (selectedCategory)
            {
                case "RecordStatus":
                    cmbboxcustomerFilterValue.DataSource = Enum.GetValues(typeof(Core.Enums.RecordStatus));
                    break;

                case "DeviceType":
                    cmbboxcustomerFilterValue.DataSource = Enum.GetValues(typeof(Core.Enums.DeviceType));
                    break;


                case "All":

                    cmbboxcustomerFilterValue.Enabled = false; // Nothing to select for "All"
                    await LoadAllCustomersAsync(); // Reload all customers
                    break;

                case "RecentActive":
                default:
                    cmbboxcustomerFilterValue.Enabled = false;
                    await LoadRecentActiveCustomersByDefaultAsync();
                    break;

               
            }
        }

        private async void cmbboxcustomerFilterValue_SelectedIndexChanged(object sender, EventArgs e)
        {

            // This event fires when the user selects a specific value (e.g., "Active" or "Laptop")
            if (cmbboxcustomerFilterValue.SelectedItem == null || cmbboxcustomerFilterCategory.SelectedValue == null) return;

            string category = cmbboxcustomerFilterCategory.SelectedValue.ToString();
            object value = cmbboxcustomerFilterValue.SelectedItem;

            List<Customer> filteredCustomers = new List<Customer>();

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
                    switch (category)
                    {
                        case "RecordStatus":
                            filteredCustomers = await customerService.GetCustomersByRecordStatus((Core.Enums.RecordStatus)value);
                            break;
                        case "DeviceType":
                            filteredCustomers = await customerService.FilterCustomersByDeviceType((Core.Enums.DeviceType)value);
                            break;
                    }
                }
                BindCustomersToGrid(filteredCustomers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filter: {ex.Message}", "Error");
            }
        }


        private async Task LoadAllCustomersAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
                    //if searchbox is not empty, filter by search term
                    if (!string.IsNullOrWhiteSpace(searchboxCustomerSection.Text))
                    {
                        var searchTerm = searchboxCustomerSection.Text.Trim();
                        var searchedCus = await customerService.SearchCustomersAsync(searchTerm);
                        BindCustomersToGrid(searchedCus.ToList());
                        return; // Exit after search
                    }
                    var customers = await customerService.GetAllCustomers();
                    // Note: GetAllCustomers returns IEnumerable, so need ToList()
                    BindCustomersToGrid(customers.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error");
            }
        }


        private void BindCustomersToGrid(List<Customer> customers)
        {
            var customerViewModels = customers.Select(c => new CustomerViewModel
            {
                Id = c.CustomerId,
                FullName = $"{c.FirstName} {c.LastName}",
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address,
                Added = c.Created,
                Status = c.RecordStatus.ToString() // Convert enum to string for display
            }).ToList();

            // Check if gridview needs to be cleared (can cause flicker)
            // A better approach is to set binding source's datasource

            gridviewCustomer.DataSource = customerViewModels;

        }

        private void searchboxCustomerSection_TextChanged(object sender, EventArgs e)
        {
            //call the method to refresh the grid view with the search term
            _ = LoadAllCustomersAsync(); // Refresh grid view with search term

        }


        //private async void RefreshAndSwitchBack()
        //{
        //    await RefreshGridView(); // Refresh data
        //    _customerSection.SwitchCustomerSection(this); // Switch back to this grid view
        //}
    }
}
