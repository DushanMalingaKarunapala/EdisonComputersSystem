using BLL.Services.CustomerService;
using BLL.Services.DeviceService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class Login : Form
    {
      
        private readonly IServiceProvider _serviceProvider; // Assuming you have a service provider for dependency injection

        public Login(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            // Initialize the service provider
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));


            //this.IsMdiContainer = true;
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm(_serviceProvider);

            //set the login form to start in the center of the screen

            loginForm.StartPosition = FormStartPosition.CenterScreen;

            //show the login form as a dialog
            loginForm.ShowDialog(this);


        }

        private void btnLoginSectionClose_Click(object sender, EventArgs e)
        {
            // close the application when the close button is clicked
            Application.Exit();
        }

        private void btnLoginSectionMaximize_Click(object sender, EventArgs e)
        {


            //maimize the application when the maximize button is clicked
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnLoginSectionMinimize_Click(object sender, EventArgs e)
        {
            // minimize the application when the minimize button is clicked
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
    }
}
