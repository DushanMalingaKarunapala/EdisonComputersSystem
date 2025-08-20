using BLL.Services.CustomerService;
using Presentation.SubSections.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Sections.Customers
{
    public partial class CustomerSection : UserControl
    {
        //store instance of main section user controls

        //private readonly CustomerSection _customerSection = new CustomerSection();
        private readonly CustomerGridView _customerGridView;
        private readonly AddCustomerSection _addCustomerSection;
        private readonly IServiceProvider _serviceProvider;

        private UserControl currentsubSection = null;


        public CustomerSection(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;

            //initialize the add customer section with the customer service
            _addCustomerSection = new AddCustomerSection(_serviceProvider, RefreshAndShowGrid);
            //initialize the customer grid view with the customer service
            _customerGridView = new CustomerGridView( this, _serviceProvider, RefreshAndShowGrid);
            //set up the main section panel
            panelCustomerSection.Dock = DockStyle.Bottom;
            //default to the customer grid view

            SwitchCustomerSection(_customerGridView);
           
        }

        public void SwitchCustomerSection(UserControl newSection)
        {
            if (currentsubSection != null)
            {
                panelCustomerSection.Controls.Remove(currentsubSection);
            }

            //add the new section to the panel
            currentsubSection = newSection;
            currentsubSection.Dock = DockStyle.Bottom;
            panelCustomerSection.Controls.Add(currentsubSection);
        }

        private void btnaddCustomer_Click(object sender, EventArgs e)
        {
            SwitchCustomerSection(_addCustomerSection);

            
        }



        // This method is called to refresh the customer grid view and switch back to it
        public async void RefreshAndShowGrid()
        {
            await _customerGridView.RefreshGridView(); // Tell the grid to refresh its data
            SwitchCustomerSection(_customerGridView);  // Then switch back to the grid view
        }

    }
}
