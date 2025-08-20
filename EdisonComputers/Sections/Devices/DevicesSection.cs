using BLL.Services.CustomerService;
using BLL.Services.DeviceService;
using Presentation.SubSections.Customers;
using Presentation.SubSections.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Sections.Devices
{
    public partial class DevicesSection : UserControl
    {
        //store instance of main section user controls

        private readonly DeviceGridViewSection _devicesGridView;
        private readonly AddDeviceSection _addDeviceSection;
        private readonly IServiceProvider _serviceProvider;
        private UserControl currentsubSection = null;

        public DevicesSection(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;

            //initialize the add device section with the customer service
            _addDeviceSection = new AddDeviceSection(_serviceProvider, RefreshAndShowGrid);

            _devicesGridView = new DeviceGridViewSection( this, _serviceProvider, RefreshAndShowGrid);

            //set up the main section panel
            paneldeviceSection.Dock = DockStyle.Fill;

            //default to the device grid view
            SwitchDeviceSection(_devicesGridView);
           
        }


        public void SwitchDeviceSection(UserControl newSection)
        {
            if (currentsubSection != null)
            {
                paneldeviceSection.Controls.Remove(currentsubSection);
            }
            //add the new section to the panel
            currentsubSection = newSection;
            currentsubSection.Dock = DockStyle.Fill;
            paneldeviceSection.Controls.Add(currentsubSection);
        }

        private void btnaddDevice_Click(object sender, EventArgs e)
        {
            SwitchDeviceSection(_addDeviceSection);
        }

        public async void RefreshAndShowGrid()
        {
            await _devicesGridView.RefreshGridView(); // Tell the grid to refresh its data
            SwitchDeviceSection(_devicesGridView);  // Then switch back to the grid view
        }

    }
}
