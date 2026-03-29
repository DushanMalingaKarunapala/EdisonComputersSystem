using BLL.Services.CustomerService;
using BLL.Services.DeviceService;
using Core.Enums;
using Core.Helpers;
using Core.IService;
using Core.Models;
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

namespace Presentation.SubSections.Devices
{
    public partial class AddDeviceSection : UserControl
    {


        private readonly IServiceProvider serviceProvider; // Service provider for dependency injection
        //callback to the device grid view to refresh the data after adding a new device
        private readonly Action _callback;

        //private field to store device image paths
        private List<string> _selecteddeviceImagePaths = new List<string>();


        public AddDeviceSection(IServiceProvider serviceProvider, Action callback)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            //set the section to load event
            this.Load += AddDeviceSection_Load;
            _callback = callback;
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }


        //method to populate the customer combobox and device type combobox when the section loads
        private async void AddDeviceSection_Load(object sender, EventArgs e)
        {

            // 1. First, tell the control to allow typing.
            cmbboxDevselectCustomer1.DropDownStyle = ComboBoxStyle.DropDown;

            // 2. Next, tell it WHERE its suggestion list comes from.
            cmbboxDevselectCustomer1.AutoCompleteSource = AutoCompleteSource.ListItems;

            // 3. FINALLY, tell it HOW to perform the autocompletion.
            cmbboxDevselectCustomer1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //populate the customer combobox with customer data
            //populate the device type combobox with device type data
            //this will be done by calling the respective services to get the data and then binding it to the comboboxes
            //example:
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
                   

                    var customers = await customerService.GetAllCustomers();
                    string fullname = customers.FirstOrDefault()?.FirstName + " " + customers.FirstOrDefault()?.LastName;
                    cmbboxDevselectCustomer1.DataSource = customers;
                    cmbboxDevselectCustomer1.DisplayMember = "FullName";
                    cmbboxDevselectCustomer1.ValueMember = "CustomerId";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading customers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }







            //get device types from the device type enum and bind to the combobox
            cmbboxDevselectType.DataSource = Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>().Select(dt => new { Id = (int)dt, Name = dt.ToString() }).ToList();

            cmbboxDevselectType.DisplayMember = "Name";

            cmbboxDevselectType.ValueMember = "Id";

            cmbboxDevselectCustomer1.SelectedIndex = -1;



        }

        private void btndeviceuploadImages_Click(object sender, EventArgs e)
        {
            // Open file dialog to select images for the device
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Add the newly selected files to our list
                    // We use ToList() to prevent issues with modifying the collection while iterating if needed
                    var newFiles = openFileDialog.FileNames.ToList();

                    // Optional: Prevent adding duplicate paths
                    foreach (var file in newFiles)
                    {
                        if (!_selecteddeviceImagePaths.Contains(file))
                        {
                            _selecteddeviceImagePaths.Add(file);
                        }
                    }

                    // Refresh the entire preview display
                    RefreshImagePreviews();
                }
            }
        }

        private void PictureBox_Click(object? sender, EventArgs e)
        {
            // Cast the sender to the PictureBox that was clicked
            Guna.UI2.WinForms.Guna2PictureBox clickedPictureBox = sender as Guna.UI2.WinForms.Guna2PictureBox;
            if (clickedPictureBox != null)
            {
                // Get the image path we stored in the Tag property
                string imagePathToRemove = clickedPictureBox.Tag.ToString();

                // Remove the path from our master list
                _selecteddeviceImagePaths.Remove(imagePathToRemove);

                // Remove the PictureBox from the display
                flppreviewAddingDeviceImages.Controls.Remove(clickedPictureBox);

                // Dispose of the control to free up memory
                clickedPictureBox.Dispose();
            }
        }


        private void ShowImageInPopup(string imagePath)
        {
            // Creates a temporary form to host the popup image
            using (Form popup = new Form
            {
                Text = "Image Preview", // Set a title for the popup window
                Width = 800,
                Height = 600,
                FormBorderStyle = FormBorderStyle.Sizable, // Allow resizing
                StartPosition = FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = true
            })
            {
                // Creates a PictureBox to display the image
                var pictureBox = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    Image = Image.FromFile(imagePath),
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                popup.Controls.Add(pictureBox);
                popup.ShowDialog(); // Show as a dialog to block interaction with the main form
            }
        }



        private void RefreshImagePreviews()
        {
            // 1. Clear all existing controls from the panel
            flppreviewAddingDeviceImages.Controls.Clear();

            // 2. Loop through the current list of selected image paths
            foreach (string imagePath in _selecteddeviceImagePaths)
            {
                // 3. Create a container Panel for the image and its delete button
                var imagePanel = new Guna.UI2.WinForms.Guna2Panel
                {
                    Size = new Size(110, 110),
                    Margin = new Padding(5),
                    BorderRadius = 8,
                    FillColor = Color.Transparent
                };

                // 4. Create the PictureBox for the image preview
                var pictureBox = new Guna.UI2.WinForms.Guna2PictureBox
                {
                    Dock = DockStyle.Fill,
                    Image = Image.FromFile(imagePath),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Cursor = Cursors.Hand,
                    Tag = imagePath, // Tag with the path for the popup
                    BorderRadius = 8
                };

                // 5. Create the 'X' delete button
                var deleteButton = new Guna.UI2.WinForms.Guna2CircleButton
                {
                    Text = "X",
                    Size = new Size(22, 22),
                    FillColor = Color.FromArgb(200, Color.Red), // Semi-transparent red
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    Location = new Point(imagePanel.Width - 25, 2), // Position at top-right
                    Tag = imagePath, // Tag with the path for deletion logic
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right // Keep it in the corner
                };

                // 6. Wire up the events
                // Clicking the picture box opens the popup
                pictureBox.Click += (s, args) => ShowImageInPopup(imagePath);

                // Clicking the delete button removes the image
                deleteButton.Click += (s, args) =>
                {
                    if (deleteButton.Tag is string pathToRemove)
                    {
                        _selecteddeviceImagePaths.Remove(pathToRemove); // Remove from data list
                        RefreshImagePreviews(); // Re-render the entire preview panel
                    }
                };

                // 7. Add controls to the panel and the panel to the FlowLayout
                imagePanel.Controls.Add(pictureBox);
                imagePanel.Controls.Add(deleteButton);
                deleteButton.BringToFront(); // Ensure the 'X' is on top

                flppreviewAddingDeviceImages.Controls.Add(imagePanel);
            }
        }













        // Event handler for the Save button click event
        private async void btndeviceSave_Click(object sender, EventArgs e)
        {

            try
            {
                //set the ui to disabled to prevent multiple clicks
                SetUIEnabled(false);


                // Enhanced validation
                var validationErrors = new List<string>();

                //if the selected customer is empty or record status is deleted show error message
                if (cmbboxDevselectCustomer1.SelectedItem == null || ((Customer)cmbboxDevselectCustomer1.SelectedItem).RecordStatus == RecordStatus.Deleted)
                    validationErrors.Add("Please select a valid customer (not deleted).");


                if (string.IsNullOrWhiteSpace(txtboxdeviceModel.Text))
                    validationErrors.Add("Device Model is required.");

                if (cmbboxDevselectCustomer1.SelectedItem == null)
                    validationErrors.Add("Please select a customer.");

                if (cmbboxDevselectType.SelectedItem == null)
                    validationErrors.Add("Please select a device type.");



                // Validate Brand if not a new build
                //if (!chkIsNewBuild.Checked && string.IsNullOrWhiteSpace(txtboxdeviceBrand.Text))
                //    validationErrors.Add("Device Brand is required for existing devices.");

                // Validate Serial Number uniqueness if provided
                if (!string.IsNullOrWhiteSpace(txtboxdeviceSerialNum.Text))
                {
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
                        var allDevices = await deviceService.GetAllDevices();
                        if (allDevices.Any(d => d.SerialNumber == txtboxdeviceSerialNum.Text.Trim()))
                        {
                            validationErrors.Add("A device with this serial number already exists.");
                        }
                    }
                }

                if (validationErrors.Any())
                {
                    MessageBox.Show(string.Join("\n", validationErrors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SetUIEnabled(true);
                    return;
                }

                //create a new device object from the input fields

                var newDevice = new Device
                {
                    Model = txtboxdeviceModel.Text.Trim(),
                    Brand = txtboxdeviceBrand.Text.Trim(),
                    SerialNumber = txtboxdeviceSerialNum.Text.Trim(),
                    Description = txtboxdeviceDescription.Text,
                    CustomerId = ((Customer)cmbboxDevselectCustomer1.SelectedItem).CustomerId, // Assuming Customer class has a CustomerId property
                    DeviceType = (DeviceType)cmbboxDevselectType.SelectedValue, // Assuming DeviceType is an enum
                    //for the recieved date use current date and time instead of the datetimepicker value
                    ReceivedDate = DateTime.Now, // Use current date and time for the received date
                    //ReceivedDate = datetimepickerdeviceRecievedDate.Value,
                    //DeviceImages = _selecteddeviceImagePaths.Select(path => new DeviceImage { ImagePath = path }).ToList() // Convert selected image paths to DeviceImage objects
                    DeviceImages = new List<DeviceImage>() // Initialize an empty list; images will be saved after device creation
                };


                using (var scope = serviceProvider.CreateScope())
                {
                    // Get the device service from the service provider
                    var _deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
                    // SAVE DEVICE FIRST TO GET THE ID

                    await _deviceService.AddDevice(newDevice.CustomerId, newDevice);

                    // Now save each image using the ImageManager and associate with the device ID
                    //newly added code from here-------------
                    var deviceImages = new List<DeviceImage>();
                    foreach (string selectedPath in _selecteddeviceImagePaths)
                    {
                        try
                        {
                            // Save image to managed folder and get relative path
                            string relativePath = ImageManager.SaveDeviceImage(newDevice.DeviceId, selectedPath);

                            deviceImages.Add(new DeviceImage
                            {
                                ImagePath = relativePath,
                                DeviceId = newDevice.DeviceId
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to save image {Path.GetFileName(selectedPath)}: {ex.Message}",
                                           "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    // Add images to device if any were successfully saved
                    if (deviceImages.Any())
                    {
                        await _deviceService.AddImages(newDevice.DeviceId, deviceImages);
                    }
                    //newly added code ends here---------------- -

                    _callback?.Invoke(); // Call the callback to refresh the device grid view after adding a new device


                }


                // Show success message and clear the form
                MessageBox.Show("Device added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the input fields after saving
                ClearFields();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the device: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                SetUIEnabled(true); // Re-enable the UI after the operation is complete
            }


        }




        //clear the input fields after saving the device
        private void ClearFields()
        {
            txtboxdeviceModel.Clear();
            txtboxdeviceBrand.Clear();
            txtboxdeviceSerialNum.Clear();
            txtboxdeviceDescription.Clear();
            cmbboxDevselectCustomer1.SelectedIndex = -1;
            cmbboxDevselectType.SelectedIndex = -1;
            datetimepickerdeviceRecievedDate.Value = DateTime.Now;
            _selecteddeviceImagePaths.Clear(); // Clear the selected image paths
            flppreviewAddingDeviceImages.Controls.Clear();

        }


        //set the UI enabled or disabled
        private void SetUIEnabled(bool enabled)
        {
            txtboxdeviceModel.Enabled = enabled;
            txtboxdeviceBrand.Enabled = enabled;
            txtboxdeviceSerialNum.Enabled = enabled;
            txtboxdeviceDescription.Enabled = enabled;
            cmbboxDevselectCustomer1.Enabled = enabled;
            cmbboxDevselectType.Enabled = enabled;
            datetimepickerdeviceRecievedDate.Enabled = enabled;
            btndeviceuploadImages.Enabled = enabled;
            btndeviceSave.Enabled = enabled;
        }

        private void btndeviceBack_Click(object sender, EventArgs e)
        {
            // Call the callback to refresh the device grid view after adding a new device
            _callback?.Invoke();
            // Optionally, you can clear the fields or reset the form here if needed
            ClearFields();
        }

        private async void chkIsNewBuild_CheckedChanged(object sender, EventArgs e)
        {
            bool isNewBuild = chkIsNewBuild.Checked;


            // Toggle visibility and pre-fill fields based on the checkbox state

            txtboxdeviceBrand.Enabled = !isNewBuild;
            txtboxdeviceModel.Enabled = !isNewBuild;
            txtboxdeviceSerialNum.Enabled = !isNewBuild;
            // You could also make them ReadOnly and change BackColor for a "disabled" look
            txtboxdeviceBrand.ReadOnly = isNewBuild;
            txtboxdeviceBrand.BackColor = isNewBuild ? SystemColors.Control : SystemColors.Window;

            txtboxdeviceModel.ReadOnly = isNewBuild;
            txtboxdeviceModel.BackColor = isNewBuild ? SystemColors.Control : SystemColors.Window;

            txtboxdeviceSerialNum.ReadOnly = isNewBuild;
            txtboxdeviceSerialNum.BackColor = isNewBuild ? SystemColors.Control : SystemColors.Window;


            if (isNewBuild)
            {
                // Pre-fill fields to create a "PC Build/ or new component purchase" dummy device automatically
                txtboxdeviceBrand.Text = "Custom Build/Purchase";
                txtboxdeviceModel.Text = $"PC Build/Purchase for {cmbboxDevselectCustomer1.Text}"; //use the selected customer's name
                cmbboxDevselectType.SelectedItem = Core.Enums.DeviceType.Other; // Set a default device type for new builds or purchases

                //create a dummy serial number for new builds because serial number should be unique and this is a new build
                //lets create build1 build2 etc. for new builds
                // Call the service to get the next serial number and assign it to the serial number textbox
                using (var scope = serviceProvider.CreateScope())
                {
                    var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
                    // Await the service call
                    txtboxdeviceSerialNum.Text = await deviceService.GenerateNextBuildSerialNumberAsync();
                }


                // txtboxdeviceSerialNum.Text = $"{serialPrefix}{buildCount}"; // Set the serial number to BUILD1, BUILD2, etc.

                //optionally disable the device type combobox for new builds
                cmbboxDevselectType.Enabled = false; // Disable the device type selection for new builds
            }
            else
            {
                // Clear the pre-filled fields when unchecked
                cmbboxDevselectType.Enabled = true;
                txtboxdeviceBrand.Text = "";
                txtboxdeviceModel.Text = "";
                txtboxdeviceSerialNum.Text = "";
                //txtboxDescription.Text = "";
            }

        }

        private void paneladdDevicenewSection_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btndeviceCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}
