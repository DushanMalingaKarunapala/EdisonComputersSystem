using BLL.Services.DeviceService;
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
    public partial class ViewDeviceSection : UserControl
    {
        private readonly IServiceProvider _serviceProvider; //service provider to resolve dependencies
        private readonly int _deviceId; //private field to hold the device ID
        private readonly Action _callback; //callback to refresh the device grid view after viewing a device

        public ViewDeviceSection(IServiceProvider serviceProvider, int deviceId, Action callback)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _deviceId = deviceId; //initialize the device ID
            this.Load += ViewDeviceSection_Load; // Subscribe to the Load event

            _callback = callback ?? throw new ArgumentNullException(nameof(callback)); //initialize the callback
        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private async void ViewDeviceSection_Load(object sender, EventArgs e)
        {

            //create a new DI scope to resolve the DeviceService dependency
            //
            using (var scope = _serviceProvider.CreateScope())
            {

                //resoleve the DeviceService from the DI container
                var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();

                try
                {

                    // Fetch the device by ID with images asynchronously
                    var device = await deviceService.GetDeviceByIdWithImages(_deviceId); // Fetch the device images asynchronously
                    if (device != null)
                    {
                        // Assuming you have labels or textboxes to display device details
                        lbldeviceID.Text = device.DeviceId.ToString();
                        lbldeviceBrand.Text = device.Brand;
                        lbldeviceModel.Text = device.Model;
                        lbldeviceSerialNumber.Text = device.SerialNumber;
                        lbldeviceDescription.Text = device.Description;
                        lbldeviceType.Text = device.DeviceType.ToString();
                        lbldevicerecievedDate.Text = device.ReceivedDate.ToString("MM/dd/yyyy");

                        // Clear previous images
                        deviceimageflowLayout.Controls.Clear();

                        // It's better to load images asynchronously IF POSSIBLE,
                        // but Image.FromFile is synchronous.
                        // Consider doing this part NOT on the UI thread if it's slow.
                        // For now, keeping it simple:
                        foreach (var img in device.DeviceImages) // This collection is now populated
                        {
                            var pictureBox = new PictureBox
                            {
                                Width = 100,
                                Height = 100,
                                SizeMode = PictureBoxSizeMode.Zoom,
                                Margin = new Padding(5),
                                Cursor = Cursors.Hand
                            };

                            // IMPORTANT: Path.Combine needs to be robust.
                            // Assuming "C:\Users\Asus\Downloads" is a base path for images
                            // and img.ImagePath is "device1_front.jpg" for example.
                            // If img.ImagePath is an ABSOLUTE path, then just use img.ImagePath.
                            string imagePath = Path.Combine("C:\\Users\\Asus\\Downloads", img.ImagePath);
                            // If img.ImagePath is already absolute:
                            // string imagePath = img.ImagePath;


                            if (File.Exists(imagePath))
                            {
                                try
                                {
                                    // Load the image
                                    pictureBox.Image = Image.FromFile(imagePath);

                                    pictureBox.Click += (s, args) => ShowImageInPopup(imagePath);
                                    deviceimageflowLayout.Controls.Add(pictureBox);
                                }
                                catch (OutOfMemoryException ex) // File is not a valid image or is corrupted
                                {
                                    Console.WriteLine($"Error loading image {imagePath}: {ex.Message}. File might be corrupted or not an image.");
                                    // Optionally add a placeholder error image to pictureBox
                                }
                                catch (FileNotFoundException ex) // Should be caught by File.Exists, but good practice
                                {
                                    Console.WriteLine($"Image file not found {imagePath}: {ex.Message}.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Image file not found: {imagePath}");
                                // Optionally add a placeholder "image not found" image
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

                    // Log the full exception, including inner exceptions and stack trace, for better debugging
                    Console.WriteLine($"ERROR in ViewDeviceSection_Load: {ex.ToString()}");
                    MessageBox.Show($"An error occurred while loading device data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    // Dispose of the service scope to free resources
                    scope.Dispose();
                }
            }

        }

        private void deviceviewsectionBack_Click(object sender, EventArgs e)
        {
            // call the callback to refresh the device grid view
            _callback?.Invoke();
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
                SizeMode = PictureBoxSizeMode.Zoom
            };

            popup.Controls.Add(pictureBox);
            popup.ShowDialog();
        }

    }
}
