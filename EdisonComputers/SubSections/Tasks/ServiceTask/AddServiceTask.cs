using BLL.Services.ServiceRecordService;
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

namespace Presentation.SubSections.Tasks.ServiceTask
{
    public partial class AddServiceTask : UserControl
    {
        private IServiceProvider serviceProvider;
        private int jobId;
        private Action callback;


        public AddServiceTask(IServiceProvider serviceProvider, int jobId, Action callback)
        {
            InitializeComponent();

            this.serviceProvider = serviceProvider;
            this.jobId = jobId;
            this.callback = callback;
        }

        private void AddServiceTask_Load(object sender, EventArgs e)
        {
            cmbboxservicetaskaddsectionServiceType.DataSource = Enum.GetValues(typeof(Core.Enums.ServiceType));

        }

        private async void btnaddservicetask_Click(object sender, EventArgs e)
        {

            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    // Create an instance of the ServiceTaskService
                    var serviceTaskService = scope.ServiceProvider.GetRequiredService<ServiceRecordService>();
                    // Create a new ServiceTask object and populate it with data from the form
                    var serviceTask = new Core.Models.ServiceRecord
                    {
                        JobId = jobId,
                        ServiceType = (Core.Enums.ServiceType)cmbboxservicetaskaddsectionServiceType.SelectedItem,
                        ServiceDescription = txtboxservicetaskaddsectionServiceDescription.Text,
                        Note = txtboxservicetaskaddsectionServiceNotes.Text,

                    };
                    // Save the service task using the service
                    await serviceTaskService.AddServiceRecordAsync(serviceTask);
                    // Optionally, you can reset the form or show a success message
                    MessageBox.Show("Service task saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //callback to the service task grid view
                    callback?.Invoke();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the service task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnservicetaskaddSectionBack_Click(object sender, EventArgs e)
        {
            callback?.Invoke();
        }
    }
}
