using BLL.Services.ServiceRecordService;
using BLL.Services.UpgradeRecordService;
using Core.Models;
using Core.ViewModels;
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
    public partial class EditServiceTask : UserControl
    {
        private IServiceProvider serviceProvider;
        private int jobId;
        private int serviceTaskId;
        private Action callback;

        private ServiceRecord _loadedServiceReocrd;


        public EditServiceTask(IServiceProvider serviceProvider, int jobId, int serviceTaskId, Action callback)
        {
            InitializeComponent();


            this.serviceProvider = serviceProvider;
            this.jobId = jobId;
            this.serviceTaskId = serviceTaskId;
            this.callback = callback;
        }

        private async void EditServiceTask_Load(object sender, EventArgs e)
        {
            await LoadServiceTask(serviceTaskId);

        }

        private void btnservicetaskeditSectionBack_Click(object sender, EventArgs e)
        {
            callback?.Invoke();
        }

        private async Task LoadServiceTask(int serviceTaskId)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var serviceRecordService = scope.ServiceProvider.GetRequiredService<ServiceRecordService>();
                    _loadedServiceReocrd = await serviceRecordService.GetServiceRecordByIdAsync(serviceTaskId);
                    if (_loadedServiceReocrd != null)
                    {
                        // Populate the form fields with the service task data
                        cmbboxservicetaskeditsectionServiceType.DataSource = Enum.GetValues(typeof(Core.Enums.ServiceType));
                        cmbboxservicetaskeditsectionServiceType.SelectedItem = _loadedServiceReocrd.ServiceType;

                        txtboxservicetaskeditsectionServiceDescription.Text = _loadedServiceReocrd.ServiceDescription;
                        txtboxservicetaskeditsectionServiceNotes.Text = _loadedServiceReocrd.Note;

                        cmbboxservicetaskeditsectionServiceStatus.DataSource = Enum.GetValues(typeof(Core.Enums.ServiceStatus));
                        cmbboxservicetaskeditsectionServiceStatus.SelectedItem = _loadedServiceReocrd.ServiceStatus;

                        txtboxserviceTaskEditServiceCharge.Text = _loadedServiceReocrd.ServiceCharge.ToString("F2"); // Format as currency with 2 decimal places

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading repair task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private async void btneditdservicetask_Click(object sender, EventArgs e)
        {

            setUIElementsState(false);

            if (_loadedServiceReocrd == null)
            {
                MessageBox.Show("Service task not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setUIElementsState(true);
                return;
            }

            //create service record object with scalar properties from the form

            var updatedServiceRecord = new ServiceRecord
            {
                ServiceRecordId = _loadedServiceReocrd.ServiceRecordId,
                JobId = jobId,
                ServiceType = (Core.Enums.ServiceType)cmbboxservicetaskeditsectionServiceType.SelectedItem,
                ServiceDescription = txtboxservicetaskeditsectionServiceDescription.Text,
                Note = txtboxservicetaskeditsectionServiceNotes.Text,
                ServiceStatus = (Core.Enums.ServiceStatus)cmbboxservicetaskeditsectionServiceStatus.SelectedItem,
                //addd service charge property here when it is added to the form
                ServiceCharge = txtboxserviceTaskEditServiceCharge.Text != "" ? decimal.Parse(txtboxserviceTaskEditServiceCharge.Text) : 0 // default to 0 if empty
            };

            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var serviceRecordService = scope.ServiceProvider.GetRequiredService<ServiceRecordService>();
                    await serviceRecordService.UpdateServiceRecordAsync(updatedServiceRecord);
                    MessageBox.Show("Service task updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    callback?.Invoke(); // Invoke the callback to refresh the grid view
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the service task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setUIElementsState(true);

            }
            finally
            {
                setUIElementsState(true);
            }
        }


        private void setUIElementsState(bool enabled)
        {
            cmbboxservicetaskeditsectionServiceType.Enabled = enabled;
            txtboxservicetaskeditsectionServiceDescription.Enabled = enabled;
            txtboxservicetaskeditsectionServiceNotes.Enabled = enabled;
            cmbboxservicetaskeditsectionServiceStatus.Enabled = enabled;
            btneditdservicetask.Enabled = enabled;
        }





        private void btnservicetaskeditsectionCancel_Click(object sender, EventArgs e)
        {
            //reload the service task again and reset the UI to its original state
            if (_loadedServiceReocrd != null)
            {
                cmbboxservicetaskeditsectionServiceType.SelectedItem = _loadedServiceReocrd.ServiceType;
                txtboxservicetaskeditsectionServiceDescription.Text = _loadedServiceReocrd.ServiceDescription;
                txtboxservicetaskeditsectionServiceNotes.Text = _loadedServiceReocrd.Note;
                cmbboxservicetaskeditsectionServiceStatus.SelectedItem = _loadedServiceReocrd.ServiceStatus;
            }
            else
            {
                MessageBox.Show("No service task loaded to cancel changes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
