using BLL.Services.CustomerService;
using BLL.Services.DeviceService;
using BLL.Services.UserService;
using EdisonComputers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class LoginForm : Form
    {
        string username = "admin";
        string password = "password"; // Use a secure method to store passwords in production

        private readonly CustomerService _customerService;

        private readonly DeviceService _deviceService; 

        private readonly IServiceProvider _serviceProvider; // Assuming you have a service provider for dependency injection

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );
        public LoginForm(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

          



            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            // Initialize the service provider
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            // Check if the username and password are correct
            //if (txtboxUsername.Text == username && txtboxPassword.Text == password)
            //{
            //    // If correct, show a message and close the login form
            //    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    this.DialogResult = DialogResult.OK; // Indicate success
            //    this.Close(); // Close the login form


            //    Dashboard dashboard = new Dashboard(_customerService, _deviceService, _serviceProvider); // Assuming you have a Dashboard form
            //    dashboard.Show(); // Show the dashboard form
            //}
            //else
            //{
            //    // If incorrect, show an error message
            //    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}


            string username = txtboxUsername.Text;
            string password = txtboxPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isvalid = false;

            try
            {
                using(var scope = _serviceProvider.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
                    isvalid =  await userService.AuthenticateUserAsync(username, password);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred while authenticating: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (isvalid)
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK; // Indicate success
                                                     //    this.Close(); // Close the login form
                this.Close();

                Dashboard dashboard = new Dashboard(_serviceProvider); // Assuming you have a Dashboard form
                dashboard.Show(); // Show the dashboard form
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
