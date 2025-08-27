using BLL.Services.CustomerService;
using BLL.Services.DeviceService;
using Core.Models;
using Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Sections.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.SubSections.Devices
{
    public partial class DeviceGridViewSection : UserControl
    {

        private readonly DevicesSection _devicesSection;
        private readonly IServiceProvider _serviceProvider; // service provider to resolve dependencies
        private readonly Action _callback; // callback to refresh the device grid view after viewing a device


        public DeviceGridViewSection(DevicesSection deviceSection, IServiceProvider serviceProvider, Action callback)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider)); // ensure service provider is not null


            _devicesSection = deviceSection;
            _callback = callback ?? throw new ArgumentNullException(nameof(callback)); // ensure callback is not null

        }



        private void gridviewDevice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void DeviceGridViewSection_Load(object sender, EventArgs e)
        {
            // Set up the grid view columns
            SetupGridViewColumns();
            // Populate filter categories
            PopulateFilterCategories();
            // Load all devices initially
            //await LoadAllDevicesAsync();
            // Wire up the cell click event handler
            //gridviewDevice.CellClick += gridviewDevice_CellClick;
            //// Wire up the filter value change event handler
            //cmbboxdevicegridviewfilterValue.SelectedIndexChanged += cmbboxdevicegridviewfilterValue_SelectedIndexChanged;
            await LoadRecentDevicesByDefaultAsync(); // Load recent devices by default
        }


        private void PopulateFilterCategories()
        {
            var categories = new List<FilterCategory>
        {
            new FilterCategory { DisplayName = "Recent Devices (30 Days)", Value = "RecentDevices" }, // New default
            new FilterCategory { DisplayName = "Show All", Value = "All" },
            new FilterCategory { DisplayName = "By Record Status", Value = "RecordStatus" },
            new FilterCategory { DisplayName = "By Device Type", Value = "DeviceType" }
        };

            cmbboxdevicegridviewfilterCateogory.DataSource = categories;
            cmbboxdevicegridviewfilterCateogory.DisplayMember = "DisplayName";
            cmbboxdevicegridviewfilterCateogory.ValueMember = "Value";

            // Wire up the event handler for when the user changes the filter category
            cmbboxdevicegridviewfilterCateogory.SelectedIndexChanged += cmbboxdevicegridviewfilterCateogory_SelectedIndexChanged;
        }





        // Load recent devices by default when the section is loaded

        private async Task LoadRecentDevicesByDefaultAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
                    var recentDevices = await deviceService.GetRecentDevicesAsync(30); // Last 30 days
                    BindDevicesToGrid(recentDevices.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading recent devices: {ex.Message}", "Error");
            }
        }










        //method to refresh the grid view data
        // A better refresh method that reloads based on current filter
        public async Task RefreshGridView()
        {

            if (cmbboxdevicegridviewfilterCateogory.SelectedValue != null)
            {
                string category = cmbboxdevicegridviewfilterCateogory.SelectedValue.ToString();

                switch (category)
                {
                    case "RecentDevices":
                        await LoadRecentDevicesByDefaultAsync();
                        break;
                    case "All":
                        await LoadAllDevicesAsync();
                        break;
                    default:
                        // For other filters with values
                        if (cmbboxdevicegridviewfilterValue.SelectedValue != null)
                        {
                            cmbboxdevicegridviewfilterValue_SelectedIndexChanged(this, EventArgs.Empty);
                        }
                        else
                        {
                            // Fallback to default if no specific value is selected
                            await LoadRecentDevicesByDefaultAsync();
                        }
                        break;
                }
            }
            else
            {
                // Fallback if no category is selected
                await LoadRecentDevicesByDefaultAsync();
            }
        }







        //add action columns to the grid view
        private void AddActionColumns()
        {
            if (gridviewDevice.Columns.Contains("View")) return; // prevent duplication
            DataGridViewImageColumn viewColumn = new DataGridViewImageColumn
            {
                Name = "View",
                HeaderText = "View",



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
            gridviewDevice.Columns.Add(viewColumn);
            gridviewDevice.Columns.Add(editColumn);
            gridviewDevice.Columns.Add(deleteColumn);
        }


        private void SetupGridViewColumns()
        {
            // Call this once,  in the UserControl's constructor or Load event.
            // Ensure it only runs once.
            if (gridviewDevice.Columns.Count > 0) return;

            gridviewDevice.AutoGenerateColumns = false; // Turn off auto-generation
            gridviewDevice.Columns.Clear();

            // Define  data columns MANUALLY
            gridviewDevice.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DeviceId",
                HeaderText = "Device Id",
                DataPropertyName = "DeviceId", // Must match property name in CustomerViewModel
                Visible = false // Often hide the ID
            });
            gridviewDevice.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CustomerName",
                HeaderText = "Customer",
                DataPropertyName = "CustomerName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            gridviewDevice.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DeviceType",
                HeaderText = "Device Type",
                DataPropertyName = "DeviceType",
                Width = 200
            });
            gridviewDevice.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SerialNumber",
                HeaderText = "Serial Number",
                DataPropertyName = "SerialNumber",
                Width = 120
            });

            gridviewDevice.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Brand",
                HeaderText = "Brand",
                DataPropertyName = "Brand",
                Width = 200
            });

            gridviewDevice.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Model",
                HeaderText = "Model",
                DataPropertyName = "Model",
                Width = 200
            });

            gridviewDevice.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RecievedDate",
                HeaderText = "Recieved Date",
                DataPropertyName = "RecievedDate",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy/MM/dd h:mm:tt", FormatProvider = System.Globalization.CultureInfo.InvariantCulture } // Format date
            });

            // NOW add  action columns
            AddActionColumns();
        }







        //handle cell click events to perform actions based on the clicked cell
        private void gridviewDevice_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Ignore header clicks

            //ensure the clicked cell is an action column

            string columnName = gridviewDevice.Columns[e.ColumnIndex].Name;

            //get the device id from the row data

            int deviceId = (int)gridviewDevice.Rows[e.RowIndex].Cells["DeviceId"].Value;

            switch (columnName)
            {
                case "View":
                    // Handle view action
                    var ViewUC = new ViewDeviceSection(_serviceProvider, deviceId, () => _devicesSection.SwitchDeviceSection(this));
                    _devicesSection.SwitchDeviceSection(ViewUC);
                    break;
                case "Edit":
                    // Handle edit action
                    var EditUc = new EditDeviceSection(_serviceProvider, deviceId, _devicesSection.RefreshAndShowGrid);
                    _devicesSection.SwitchDeviceSection(EditUc);

                    break;
                case "Delete":
                    // Handle delete action
                    var confirmResult = MessageBox.Show("Are you sure you want to delete this device?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirmResult == DialogResult.Yes)
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var devicesService = scope.ServiceProvider.GetRequiredService<DeviceService>();
                            // Call the delete method from the service
                            devicesService.DeleteDevice(deviceId);
                        }

                        MessageBox.Show("Device deleted successfully.", "Delete Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DeviceGridViewSection_Load(sender, e); // Refresh the grid view
                    }
                    break;
            }

        }

        private async void cmbboxdevicegridviewfilterCateogory_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbboxdevicegridviewfilterCateogory.SelectedValue == null) return;

            string selectedCategory = cmbboxdevicegridviewfilterCateogory.SelectedValue.ToString();

            // Clear the second ComboBox before populating
            cmbboxdevicegridviewfilterValue.DataSource = null;
            cmbboxdevicegridviewfilterValue.Enabled = true;

            switch (selectedCategory)
            {
                case "RecordStatus":
                    cmbboxdevicegridviewfilterValue.DataSource = Enum.GetValues(typeof(Core.Enums.RecordStatus));
                    break;

                case "DeviceType":
                    cmbboxdevicegridviewfilterValue.DataSource = Enum.GetValues(typeof(Core.Enums.DeviceType));
                    break;

                case "All":

                    cmbboxdevicegridviewfilterValue.Enabled = false; // Nothing to select for "All"
                    await LoadAllDevicesAsync(); // Reload all customers
                    break;



                case "RecentDevices":
                default:
                    cmbboxdevicegridviewfilterValue.Enabled = false; // No specific value needed for recent devices
                    await LoadRecentDevicesByDefaultAsync(); // Load recent devices
                    break;

               
            }
        }

        private async void cmbboxdevicegridviewfilterValue_SelectedIndexChanged(object sender, EventArgs e)
        {

            // This event fires when the user selects a specific value (e.g., "Active" or "Laptop")
            if (cmbboxdevicegridviewfilterCateogory.SelectedItem == null || cmbboxdevicegridviewfilterValue.SelectedValue == null) return;

            string category = cmbboxdevicegridviewfilterCateogory.SelectedValue.ToString();
            object value = cmbboxdevicegridviewfilterValue.SelectedItem;

            List<Device> filteredDevices = new List<Device>();

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
                    switch (category)
                    {
                        case "RecordStatus":
                            filteredDevices = await deviceService.GetDeviceByRecordStatus((Core.Enums.RecordStatus)value);
                            break;
                        case "DeviceType":
                            filteredDevices = await deviceService.GetDeviceByDeviceType((Core.Enums.DeviceType)value);
                            break;
                    }
                }
                BindDevicesToGrid(filteredDevices);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filter: {ex.Message}", "Error");
            }
        }



        private async Task LoadAllDevicesAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();

                    //if user types in the search box, filter the devices by search term
                    if (!string.IsNullOrWhiteSpace(searchboxdeviceSection.Text))
                    {
                        var searchTerm = searchboxdeviceSection.Text.Trim();
                        var filteredDevices = await deviceService.SearchDevices(searchTerm);
                        BindDevicesToGrid(filteredDevices.ToList());
                        return; // Exit after filtering
                    }


                    var devices = await deviceService.GetAllDevicesWithCustomerFullName();
                    // Note: GetAllCustomers returns IEnumerable, so need ToList()
                    BindDevicesToGrid(devices.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Devices: {ex.Message}", "Error");
            }
        }


        private void BindDevicesToGrid(List<Device> devices)
        {
            var deviceViewModels = devices.Select(c => new DeviceViewModel
            {
                DeviceId = c.DeviceId,
                CustomerName = c.Customer?.FullName ?? "Unknown", // Assuming Customer is a navigation property
                Brand = c.Brand,
                Model = c.Model,
                SerialNumber = c.SerialNumber,
                Description = c.Description,
                DeviceType = c.DeviceType,
                RecievedDate = c.ReceivedDate,
                // Assuming DeviceImages is a collection of images, you might want to handle it differently
                // UploadedImages = c.DeviceImages.Select(img => new DeviceImageViewModel { ImagePath = img.ImagePath }).ToList()   


            }).ToList();

            // Check if gridview needs to be cleared (can cause flicker)
            // A better approach is to set binding source's datasource

            gridviewDevice.DataSource = deviceViewModels;

        }

        private void searchboxdeviceSection_TextChanged(object sender, EventArgs e)
        {
            _ = LoadAllDevicesAsync(); // Reload devices based on search term
        }
    }
}
