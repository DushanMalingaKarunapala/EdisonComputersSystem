using Presentation.SubSections;
using Presentation.SubSections.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Sections.Components
{
    public partial class ComponentSection : UserControl
    {
        private UserControl currentsubSection = null;
        private readonly AddComponentsSection _addComponentsSection;
        private readonly ComponentGridView _componentGridViewSection;
        private readonly EditComponentSection _editComponentSection;
        private readonly IServiceProvider _serviceProvider;


        public ComponentSection(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            //initialize the add component section with the service provider
            _addComponentsSection = new AddComponentsSection(_serviceProvider, RefreshAndShowGrid);

            //initialize the component grid view section
            _componentGridViewSection = new ComponentGridView(_serviceProvider, this);


           

            //set up the main section panel
            panelComponentSection.Dock = DockStyle.Fill;

            //default to the component grid view
            SwitchComponentSection(_componentGridViewSection);
        }



        //methodd to switch between sections
        public void SwitchComponentSection(UserControl newSection)
        {
            if (currentsubSection != null)
            {
                panelComponentSection.Controls.Remove(currentsubSection);
            }
            //add the new section to the panel
            currentsubSection = newSection;
            currentsubSection.Dock = DockStyle.Fill;
            panelComponentSection.Controls.Add(currentsubSection);
        }

        private void btnaddComponents_Click(object sender, EventArgs e)
        {
            SwitchComponentSection(_addComponentsSection);
        }


        public async void RefreshAndShowGrid()
        {
            await _componentGridViewSection.RefreshGridView(); // Tell the grid to refresh its data
            SwitchComponentSection(_componentGridViewSection);  // Then switch back to the grid view
        }
    }



}
