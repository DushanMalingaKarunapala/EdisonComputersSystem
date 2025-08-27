using BLL.Services.CustomerService;
using BLL.Services.DeviceService;
using Presentation;
using Presentation.Sections.Components;
using Presentation.Sections.Customers;
using Presentation.Sections.Dashboard;
using Presentation.Sections.Devices;
using Presentation.Sections.Jobs;
using Presentation.Sections.User;

namespace EdisonComputers
{
    public partial class Dashboard : Form
    {
        //store instances of user controls to reuse them

        private readonly DashboardSection dashboardSection;
        private readonly UserSection userSection;
        private readonly CustomerSection customerSection;
        private readonly DevicesSection devicesSection;
        private readonly JobsSection jobsSection;
        private readonly ComponentSection componentSection;
        private readonly IServiceProvider _serviceProvider;

        private UserControl currentSection = null;

        public Dashboard(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // Set consistent scaling
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            //set minumum size to prevent  ui breaking on smaller screens
            this.MinimumSize = new Size(1200, 700);

            //handle screen size differences
            //AdjustLayoutForScreen();

            //configure Guna controls for better responsiveness
            //ConfigureGunaControls();
            //make the dashboard draggable when the user clicks and drags panelDashboardContainer or dashbordsidePanel
            guna2DragControl2.TargetControl = panelDashboardContainer;
            guna2DragControl1.TargetControl = dashbordsidePanel;

            // ADDITIONAL DRAG CONFIGURATION OPTIONS
            //guna2DragControl1.UseTransparentDrag = true; // Smooth dragging
            //guna2DragControl1.TransparentWhileDrag = true; // Form becomes transparent while dragging
            //guna2DragControl1.DragStartTransparencyValue = 80; // Transparency level (0-100)







            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));



            //initialize the dashboard section with the service provider
            dashboardSection = new DashboardSection(_serviceProvider, jobsSection);

            //initialize the user section with the service provider
            userSection = new UserSection(_serviceProvider);

            //initialize the customer section with the customer service
            customerSection = new CustomerSection(_serviceProvider);

            //initialize the devices section with the customer service

            devicesSection = new DevicesSection(_serviceProvider);

            //initialize the jobs section
            jobsSection = new JobsSection(_serviceProvider);



            //initialize the components section
            componentSection = new ComponentSection(_serviceProvider);


            //set up the container for the user controls
            panelDashboardContainer.Dock = DockStyle.Fill;

            // load the dashboard section by default


            SwitchSection(dashboardSection);

        }

        private void SwitchSection(UserControl newSection)
        {
            if (currentSection != null)
            {
                panelDashboardContainer.Controls.Remove(currentSection);
            }
            currentSection = newSection;
            currentSection.Dock = DockStyle.Fill;
            panelDashboardContainer.Controls.Add(currentSection);
            //currentSection.BringToFront();
        }


        private void btnsidepanelDashboard_Click(object sender, EventArgs e)
        {
            SwitchSection(dashboardSection);
        }

        private void btnsidepanelCus_Click(object sender, EventArgs e)
        {
            SwitchSection(customerSection);
        }

        private void btnsidepanelDevices_Click(object sender, EventArgs e)
        {
            SwitchSection(devicesSection);
        }

        private void btnsidepanelJobs_Click(object sender, EventArgs e)
        {
            SwitchSection(jobsSection);
        }

        private void btnsidepanelComponents_Click(object sender, EventArgs e)
        {
            SwitchSection(componentSection);
        }


        private void btnsidepanelUser_Click(object sender, EventArgs e)
        {
            SwitchSection(userSection);
        }





        private void btnDashboardSectionClose_Click(object sender, EventArgs e)
        {
            //close the application when the close button is clicked
            Application.Exit();
        }

        private void btnDashboardSectionMaximize_Click(object sender, EventArgs e)
        {
            //maximize the application when the maximize button is clicked
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnDashboardSectionMinimize_Click(object sender, EventArgs e)
        {
            //minimize the application when the minimize button is clicked
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            //logout the user and revert to the login screen
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //close the current dashboard and open the Login
                this.Hide();
                var login = new Login(_serviceProvider);
                login.Show();

            }
        }

        private void dashbordsidePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        //private void AdjustLayoutForScreen()
        //{
        //    var screen = Screen.FromControl(this);
        //    var workingArea = screen.WorkingArea;

        //    // Adjust form size based on screen
        //    if (workingArea.Width < 1280 || workingArea.Height < 800)
        //    {
        //        this.WindowState = FormWindowState.Maximized;
        //    }
        //    else
        //    {
        //        // Set to a reasonable size for larger screens
        //        this.Size = new Size(Math.Min(1280, workingArea.Width - 100),
        //                           Math.Min(800, workingArea.Height - 100));
        //        this.CenterToScreen();
        //    }



        //}

        //private void ConfigureGunaControls()
        //{
        //    // Apply scaling to all Guna controls
        //    foreach (Control control in this.Controls)
        //    {
        //        ConfigureGunaControl(control);
        //    }
        //}

        //private void ConfigureGunaControl(Control control)
        //{
        //    if (control is Guna.UI2.WinForms.Guna2DataGridView grid)
        //    {
        //        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        //    }

        //    if (control is Guna.UI2.WinForms.Guna2Panel panel)
        //    {
        //        panel.AutoScroll = true;
        //    }

        //    // Recursively apply to child controls
        //    foreach (Control child in control.Controls)
        //    {
        //        ConfigureGunaControl(child);
        //    }
        //}
    }
}
