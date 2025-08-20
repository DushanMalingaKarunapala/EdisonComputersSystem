using Presentation.SubSections.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Sections.Jobs
{
    public partial class JobsSection : UserControl
    {
        private readonly JobGridViewSection _jobsGridView;
        private readonly AddJobSection _addJobSection;
        private readonly IServiceProvider _serviceProvider;
       
        private UserControl currentsubSection = null;

        public JobsSection(IServiceProvider serviceProvider)
        {

            InitializeComponent();
            _serviceProvider = serviceProvider; // Assuming you don't need a service provider here, otherwise pass it as needed
            _jobsGridView = new JobGridViewSection(_serviceProvider, this);
            _addJobSection = new AddJobSection(_serviceProvider, RefreshAndShowGrid /*()=>SwitchJobSection(_jobsGridView)*/);

            //set up the main section panel
            panelJobSection.Dock = DockStyle.Fill;
            //default to the job grid view
            SwitchJobSection(_jobsGridView);

        }


        public void SwitchJobSection(UserControl newSection)
        {
            if (currentsubSection != null)
            {
                panelJobSection.Controls.Remove(currentsubSection);
            }
            //add the new section to the panel
            currentsubSection = newSection;
            currentsubSection.Dock = DockStyle.Fill;
            panelJobSection.Controls.Add(currentsubSection);


            // *** FIX #3: Explicitly tell ViewJobSection to load its data ***
            if (newSection is ViewJobSection viewJob)
            {
                viewJob.LoadJob(); // Call the public LoadJob method
            }
        }

        private void btnaddJob_Click(object sender, EventArgs e)
        {
            SwitchJobSection(_addJobSection);
        }

        private void JobsSection_Load(object sender, EventArgs e)
        {

        }



        public async void RefreshAndShowGrid()
        {
            await _jobsGridView.RefreshGridView(); // Tell the grid to refresh its data
            SwitchJobSection(_jobsGridView);  // Then switch back to the grid view
        }
    }
}
