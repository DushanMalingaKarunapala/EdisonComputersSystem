using BLL.Services.DeviceService;
using Core.Enums;
using Core.Helpers;
using Core.Models;
using Guna.UI2.Designer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
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
    public partial class EditDeviceSection : UserControl
    {
        //iservice provider to resolve dependencies
        private readonly IServiceProvider _serviceProvider;
        //private field to hold the device ID

        private readonly Action _oncompleteCallback; //callback to the device grid view to refresh the data after editing a device
        private readonly int _deviceId;

        private Device _loadeddevice; // To hold the loaded device details


        //class-level list to keep track of image IDs marked for deletion
        private List<int> _imageIdsToRemove = new List<int>();
        //class-level list to keep track of new image file paths to be added
        private List<string> _newImageFilePathsToAdd = new List<string>();





        public EditDeviceSection(IServiceProvider serviceProvider, int deviceId, Action oncompleteCallback)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _deviceId = deviceId;
            _oncompleteCallback = oncompleteCallback;
        }


        // Load the device details into the textboxes when the section is loaded
        private async void EditDeviceSection_Load(object sender, EventArgs e)
        {

            await LoadDeviceData(); // Load the device details into the textboxes for editing   


        }


        private async Task LoadDeviceData()
        {

            // Create a new DI scope to resolve the DeviceService dependency
            using (var scope = _serviceProvider.CreateScope())
            {
                // Resolve the DeviceService from the DI container
                var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();

                try
                {


                    // Fetch the device by ID with images asynchronously
                    _loadeddevice = await deviceService.GetDeviceByIdWithImages(_deviceId); // Fetch the device images asynchronously

                    if (_loadeddevice != null)
                    {
                        // Assuming you have labels or textboxes to display device details

                        txtboxdeviceBrand.Text = _loadeddevice.Brand;
                        txtboxdeviceModel.Text = _loadeddevice.Model;
                        txtboxdeviceSerialNum.Text = _loadeddevice.SerialNumber;
                        txtboxdeviceDescription.Text = _loadeddevice.Description;
                        cmbboxDevselectType.DataSource = Enum.GetValues(typeof(DeviceType));
                        cmbboxDevselectType.SelectedItem = _loadeddevice.DeviceType; // Set the selected device type
                        datetimepickerdeviceRecievedDate.Value = _loadeddevice.ReceivedDate;


                        // Clear previous images
                        deviceimageflowLayout.Controls.Clear();

                        _imageIdsToRemove.Clear(); // Clear any previously marked images
                        _newImageFilePathsToAdd.Clear(); // Clear new images list


                        // It's better to load images asynchronously IF POSSIBLE,
                        // but Image.FromFile is synchronous.
                        // Consider doing this part NOT on the UI thread if it's slow.
                        // For now, keeping it simple:
                        foreach (var img in _loadeddevice.DeviceImages) // This collection is now populated
                        {

                            // Create a container Panel for the image and its delete button
                            var imagePanel = new Guna.UI2.WinForms.Guna2Panel
                            {
                                Width = 110, // Slightly wider to accommodate delete button
                                Height = 125, // Slightly taller
                                Margin = new Padding(5),
                                BorderRadius = 10,
                                FillColor = Color.Transparent // Or a very light background
                            };




                            var pictureBox = new Guna.UI2.WinForms.Guna2PictureBox
                            {
                                Width = 100,
                                Height = 100,
                                SizeMode = PictureBoxSizeMode.Zoom,
                                Margin = new Padding(5),
                                Cursor = Cursors.Hand,
                                Tag = img.Id, // Store the DeviceImage object in the Tag property for later use
                                BorderRadius = 8,
                                Location = new Point(5, 5) // Position the pictureBox inside the panel

                            };


                            var deleteButton = new Guna.UI2.WinForms.Guna2CircleButton // Using Guna Circle Button for a small 'x'
                            {
                                Text = "X", // Or use an icon
                                            // Image = Properties.Resources.DeleteIcon, // If you have an icon
                                            // ImageSize = new Size(12,12),
                                Size = new Size(20, 20),
                                FillColor = Color.Red,
                                ForeColor = Color.White,
                                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                                Location = new Point(imagePanel.Width - 25, 2), // Top-right corner
                                Tag = img.Id, // Also tag the button with the DeviceImage ID
                                Cursor = Cursors.Hand,
                                Anchor = AnchorStyles.Top | AnchorStyles.Right // Keep it positioned correctly
                            };

                            deleteButton.Click += (s, args) =>
                            {
                                // Handle delete button click
                                var btn = s as Guna.UI2.WinForms.Guna2CircleButton;
                                if (btn?.Tag is int imageIdToDelete)
                                {
                                    var result = MessageBox.Show("Are you sure you want to remove this image?", "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                    if (result == DialogResult.Yes)
                                    {
                                        // Add to removal list and visually remove (or strike-through)
                                        if (!_imageIdsToRemove.Contains(imageIdToDelete))
                                        {
                                            _imageIdsToRemove.Add(imageIdToDelete);
                                        }
                                        // Visually remove the panel containing the image and button
                                        deviceimageflowLayout.Controls.Remove(imagePanel);
                                        imagePanel.Dispose(); // Dispose the panel and its children
                                    }
                                }
                            };


                            //string imagePath = Path.Combine("C:\\Users\\Asus\\Downloads", img.ImagePath);

                            //if (File.Exists(imagePath))
                            //{
                            //    try
                            //    {
                            //        // Load the image
                            //        pictureBox.Image = Image.FromFile(imagePath);

                            //        pictureBox.Click += (s, args) => ShowImageInPopup(imagePath);

                            //        imagePanel.Controls.Add(pictureBox);
                            //        imagePanel.Controls.Add(deleteButton); // Add delete button to the panel

                            //        deviceimageflowLayout.Controls.Add(imagePanel);
                            //    }
                            //    catch (Exception ex) // File is not a valid image or is corrupted
                            //    {
                            //        Console.WriteLine($"Error loading image {imagePath}: {ex.Message}. File might be corrupted or not an image.");
                            //        // Optionally add a placeholder error image to pictureBox

                            //        // Optionally add a placeholder for broken images
                            //        var errorLabel = new Label { Text = "Error", Size = new Size(100, 100), TextAlign = ContentAlignment.MiddleCenter, BorderStyle = BorderStyle.FixedSingle };
                            //        imagePanel.Controls.Add(errorLabel);
                            //        imagePanel.Controls.Add(deleteButton); // Still allow deleting the record
                            //        deviceimageflowLayout.Controls.Add(imagePanel);


                            //    }

                            //}
                            //else
                            //{
                            //    Console.WriteLine($"Image file not found: {imagePath}");
                            //    // Optionally add a placeholder "image not found" image

                            //    var notFoundLabel = new Label { Text = "Not Found", Size = new Size(100, 100), TextAlign = ContentAlignment.MiddleCenter, BorderStyle = BorderStyle.FixedSingle };
                            //    imagePanel.Controls.Add(notFoundLabel);
                            //    imagePanel.Controls.Add(deleteButton); // Still allow deleting the record
                            //    deviceimageflowLayout.Controls.Add(imagePanel);
                            //}


                            //use the ImageManager to get the full image path--------------------------- changed from here
                            string imagePath = Core.Helpers.ImageManager.GetFullImagePath(img.ImagePath);

                            if (ImageManager.ImageExists(img.ImagePath))
                            {
                                try
                                {
                                    // Load the image without locking the file
                                    pictureBox.Image = LoadImageWithoutLocking(imagePath);
                                    pictureBox.Click += (s, args) => ShowImageInPopup(imagePath);

                                    // Fix the control hierarchy too
                                    imagePanel.Controls.Add(pictureBox);
                                    imagePanel.Controls.Add(deleteButton);
                                    deleteButton.BringToFront();
                                    deviceimageflowLayout.Controls.Add(imagePanel); ;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error loading image {imagePath}: {ex.Message}");
                                    // Add placeholder for broken image
                                    var errorLabel = new Label
                                    {
                                        Text = "Error Loading",
                                        Size = new Size(100, 100),
                                        TextAlign = ContentAlignment.MiddleCenter,
                                        BorderStyle = BorderStyle.FixedSingle
                                    };
                                    deviceimageflowLayout.Controls.Add(errorLabel);
                                    imagePanel.Controls.Add(deleteButton); // Still allow deleting the record
                                    deviceimageflowLayout.Controls.Add(imagePanel); // Add the panel even if image load failed
                                }
                            }
                            else
                            {
                                // Add placeholder for missing image
                                var notFoundLabel = new Label
                                {
                                    Text = "Image Not Found",
                                    Size = new Size(100, 100),
                                    TextAlign = ContentAlignment.MiddleCenter,
                                    BorderStyle = BorderStyle.FixedSingle
                                };
                                deviceimageflowLayout.Controls.Add(notFoundLabel);
                                imagePanel.Controls.Add(deleteButton); // Still allow deleting the record
                                deviceimageflowLayout.Controls.Add(imagePanel);
                            }
                        }


                    }
                    else
                    {
                        MessageBox.Show("Device not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while loading device data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    scope.Dispose(); // Dispose of the scope to release resources
                }





            }

        }


        // this helper method will load the image without locking the file
        private Image LoadImageWithoutLocking(string imagePath)
        {
            using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                return Image.FromStream(stream);
            }
        }



        private void ShowImageInPopup(string imagePath)
        {
            Form popup = new Form
            {
                Width = 800,
                Height = 600,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            var pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = Image.FromFile(imagePath),
                SizeMode = PictureBoxSizeMode.Zoom,

                Cursor = Cursors.Hand



            };

            popup.Controls.Add(pictureBox);
            popup.ShowDialog();
        }





        private void btnupdateSectionBack_Click(object sender, EventArgs e)
        {
            //callback to go back to the device grid view section
            _oncompleteCallback?.Invoke();
        }

        private async void btnupdateDevice_Click(object sender, EventArgs e)
        {
            if (_loadeddevice == null)
            {
                MessageBox.Show("No device loaded to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //update the loaded device object with the new scalar values from the textboxes

            try
            {

                SetUIEnabled(false); // Disable UI controls while updating


                var devicetoupdate = new Device
                {
                    DeviceId = _loadeddevice.DeviceId, // Keep the same ID to update the existing device

                    Brand = txtboxdeviceBrand.Text.Trim(),
                    Model = txtboxdeviceModel.Text.Trim(),
                    SerialNumber = txtboxdeviceSerialNum.Text.Trim(),
                    Description = txtboxdeviceDescription.Text.Trim(),
                    DeviceType = (DeviceType)cmbboxDevselectType.SelectedItem, // Cast to DeviceType enum
                    ReceivedDate = datetimepickerdeviceRecievedDate.Value,
                    CustomerId = _loadeddevice.CustomerId // Keep the same customer ID
                };

                //prepare the new device image objects to add
                //var imagestoadd = _newImageFilePathsToAdd.Select(path => new DeviceImage { ImagePath = path }).ToList(); // this was commented out because we need to save the images to managed folders first now

                // ✅ PROCESS NEW IMAGES - SAVE TO MANAGED FOLDERS
                var imagestoadd = new List<DeviceImage>();

                //use the ImageManager to save each new image and get the relative path

                foreach (string newImagePath in _newImageFilePathsToAdd)
                {
                    try
                    {
                        string relativePath = ImageManager.SaveDeviceImage(_deviceId, newImagePath);
                        imagestoadd.Add(new DeviceImage { ImagePath = relativePath });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save image {Path.GetFileName(newImagePath)}: {ex.Message}",
                                       "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }



                //call the the service to update the device with images
                using (var scope = _serviceProvider.CreateScope())
                {
                    var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
                    // Call the service to update the device
                    var updatedDevice = await deviceService.UpdateDevice(devicetoupdate, imagestoadd, _imageIdsToRemove);
                    if (updatedDevice != null)
                    {

                        // ✅ DELETE REMOVED IMAGES FROM FILE SYSTEM   -----newly added code from here
                        foreach (int imageId in _imageIdsToRemove)
                        {
                            // You'll need to get the image path from the original _loadeddevice
                            var imageToDelete = _loadeddevice.DeviceImages.FirstOrDefault(img => img.Id == imageId);
                            if (imageToDelete != null)
                            {
                                ImageManager.DeleteDeviceImage(imageToDelete.ImagePath);
                            }
                        }
                        // ----- to here

                        MessageBox.Show("Device updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Clear the lists after successful update
                        _imageIdsToRemove.Clear();
                        _newImageFilePathsToAdd.Clear();
                        _oncompleteCallback?.Invoke();
                        //refresh the device grid view section using service provider


                    }
                    else
                    {
                        MessageBox.Show("Failed to update device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SetUIEnabled(true); // Re-enable UI controls if update failed
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the device: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                SetUIEnabled(true); // Re-enable UI controls after operation
            }

        }

        private void btndeviceuploadImagesEditSection_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        _newImageFilePathsToAdd.Add(filePath); // Add to your list of new files

                        // Display a preview of the newly selected image
                        // You can add these to the same deviceimageflowLayout or a separate one for "new" images
                        // ✅ COMPLETE THE STYLING:
                        var imagePanel = new Guna.UI2.WinForms.Guna2Panel
                        {
                            Size = new Size(110, 110),
                            Margin = new Padding(5),
                            BorderRadius = 8,
                            FillColor = Color.Transparent
                        };

                        var pictureBox = new Guna.UI2.WinForms.Guna2PictureBox
                        {
                            Dock = DockStyle.Fill,
                            SizeMode = PictureBoxSizeMode.Zoom,
                            Cursor = Cursors.Hand,
                            Tag = filePath,
                            BorderRadius = 8
                        };

                        var removeNewButton = new Guna.UI2.WinForms.Guna2CircleButton
                        {
                            Text = "X",
                            Size = new Size(22, 22),
                            FillColor = Color.FromArgb(200, Color.Red),
                            ForeColor = Color.White,
                            Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                            Location = new Point(imagePanel.Width - 25, 2),
                            Tag = filePath,
                            Cursor = Cursors.Hand,
                            Anchor = AnchorStyles.Top | AnchorStyles.Right
                        };


                        // Tag the remove button with the filePath so you can remove it from _newImageFilePathsToAdd
                        removeNewButton.Tag = filePath;
                        removeNewButton.Click += (s, args) =>
                        {
                            if (removeNewButton.Tag is string pathToRemove)
                            {
                                _newImageFilePathsToAdd.Remove(pathToRemove);
                                deviceimageflowLayout.Controls.Remove(imagePanel); // Or remove from wherever you display new previews
                                imagePanel.Dispose();
                            }
                        };

                        if (File.Exists(filePath))
                        {
                            try
                            {
                                pictureBox.Image = Image.FromFile(filePath);
                                pictureBox.Click += (s, args) => ShowImageInPopup(filePath);
                                imagePanel.Controls.Add(pictureBox);
                                imagePanel.Controls.Add(removeNewButton);
                                removeNewButton.BringToFront();
                                deviceimageflowLayout.Controls.Add(imagePanel);
                            }
                            catch (Exception ex) { Console.WriteLine($"Error previewing new image {filePath}: {ex.Message}"); }
                        }
                    }
                }
            }
        }


        private void SetUIEnabled(bool enabled)
        {
            // Enable or disable all controls in the section
            foreach (Control control in this.Controls)
            {
                control.Enabled = enabled;
            }
        }






        private async void btndeviceUpdCancel_Click(object sender, EventArgs e)
        {
            //reload the device again and reset the UI to its original state
            await LoadDeviceData(); // Reload the device data to reset the UI


        }
    }
}
