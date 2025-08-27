using BLL.Services.RepairRecordService;
using BLL.Services.ServiceRecordService;
using Core.Enums;
using Core.IRepository;
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
    public partial class GridViewServiceTask : UserControl
    {
        private IServiceProvider serviceProvider;
        private int jobId;
        private Action showAddServiceTaskView;
        private Action<int> showEditServiceTaskView;



        public GridViewServiceTask(IServiceProvider serviceProvider, int jobId, Action showAddServiceTaskView, Action<int> showEditServiceTaskView)
        {

            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.jobId = jobId;
            this.showAddServiceTaskView = showAddServiceTaskView;
            this.showEditServiceTaskView = showEditServiceTaskView;
        }

        private async void GridViewServiceTask_Load(object sender, EventArgs e)
        {
            SetupGridViewColumns();
            await LoadAllServiceTasksAsync(); // Load tasks asynchronously

        }

        //private async Task LoadServiceTasks()
        //{

        //    try
        //    {
        //        var serviceScope = serviceProvider.CreateScope();
        //        var genericService = serviceScope.ServiceProvider.GetRequiredService<IRepository<ServiceRecord>>();


        //        var serviceRecords = await genericService.GetAll();

        //        gridviewServiceTaskViewer.DataSource = serviceRecords
        //            .Where(st => st.JobId == jobId) // Filter tasks by jobId
        //            .Select(st => new
        //            {
        //                st.ServiceRecordId,
        //                st.ServiceDescription,
        //                st.Note,
        //                ServiceStatus = st.ServiceStatus.ToString(), // Convert enum to string for display


        //            })
        //            .ToList();

        //        AddActionColumns(); // Add action columns after setting the data source

        //    }
        //    catch
        //    {
        //        MessageBox.Show("Failed to load repair tasks. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //    }

        //}


        private void AddActionColumns()
        {
            if (gridviewServiceTaskViewer.Columns.Contains("View")) return; // prevent duplication



            DataGridViewImageColumn editColumn = new DataGridViewImageColumn
            {
                Name = "Edit",
                HeaderText = "Edit",
                Image = Resource1.Create3, // ✏️
                Width = 2,
              
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };

            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn
            {
                Name = "Delete",
                HeaderText = "Delete",
                Image = Resource1.Remove, // 🗑️
                Width = 2,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };


            gridviewServiceTaskViewer.Columns.Add(editColumn);
            gridviewServiceTaskViewer.Columns.Add(deleteColumn);
        }




        private void SetupGridViewColumns()
        {
            // Call this once,  in the UserControl's constructor or Load event.
            // Ensure it only runs once.
            if (gridviewServiceTaskViewer.Columns.Count > 0) return;

            gridviewServiceTaskViewer.AutoGenerateColumns = false; // Turn off auto-generation
            gridviewServiceTaskViewer.Columns.Clear();

            // Define  data columns MANUALLY
            gridviewServiceTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ServiceRecordId",
                HeaderText = "Service Record ID",
                DataPropertyName = "ServiceRecordId", // Must match property name in CustomerViewModel
                Visible = true // Often hide the ID
            });
            gridviewServiceTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Width = 250
            });
            gridviewServiceTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ServiceType",
                HeaderText = "Service Type",
                DataPropertyName = "ServiceType",
                Width = 200
            });
            gridviewServiceTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Note",
                HeaderText = "Notes",
                DataPropertyName = "Note",
                Width = 120
            });
            gridviewServiceTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ServiceStatus",
                HeaderText = "Service Status",
                DataPropertyName = "ServiceStatus",
                Width = 120
            });

            // NOW add  action columns
            AddActionColumns();
        }







        private async void gridviewServiceTaskViewer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Ignore header row
            // Ensure the clicked cell is one of the action columns
            string columnName = gridviewServiceTaskViewer.Columns[e.ColumnIndex].Name;

            var serviceTaskId = (int)gridviewServiceTaskViewer.Rows[e.RowIndex].Cells["ServiceRecordId"].Value;


            if (columnName == "Edit")
            {
                // Call the callback to show the edit view
                showEditServiceTaskView(serviceTaskId);
            }
            else if (columnName == "Delete")
            {
                // Handle delete action
                var confirmResult = MessageBox.Show("Are you sure you want to Cancel this update task?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    //change the status of the repair task to cancelled
                    var serviceScope = serviceProvider.CreateScope();
                    var genericService = serviceScope.ServiceProvider.GetRequiredService<IRepository<ServiceRecord>>();
                    var serviceTask = await genericService.GetById(serviceTaskId);
                    if (serviceTask != null)
                    {
                        serviceTask.ServiceStatus = Core.Enums.ServiceStatus.ServiceCancelled; // Assuming you have a Cancelled status
                        await genericService.Update(serviceTask);
                        MessageBox.Show("Service task cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshGridView(); // Refresh the grid view
                    }
                    else
                    {
                        MessageBox.Show("Service task not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }


        //a public callback method to refresh the grid view when a new service task is added or edited
        public async Task RefreshGridView()
        {
            await LoadAllServiceTasksAsync(); // Reload the service tasks
        }



        private async Task LoadAllServiceTasksAsync()
        {
            try
            {
                using (var scope = this.serviceProvider.CreateScope())
                {
                    var servicerecordService = scope.ServiceProvider.GetRequiredService<ServiceRecordService>();
                    var servicerecords = await servicerecordService.GetServiceRecordsByJobIdAsync(jobId);
                    // Note: GetAllCustomers returns IEnumerable, so need ToList()
                    BindRepairTasksToGrid(servicerecords.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Service Tasks: {ex.Message}", "Error");
            }
        }




        private void BindRepairTasksToGrid(List<ServiceRecord> serviceRecords)
        {
            SetupGridViewColumns(); // Ensure columns are set up before binding data
            var servicerViewModels = serviceRecords
                .Where(rr => rr.JobId == jobId)
                .Select(c => new ServiceRecordViewModel
                {
                    ServiceRecordId = c.ServiceRecordId,
                    Description = c.ServiceDescription,
                    Note = c.Note,
                    ServiceType = c.ServiceType.ToString(), // Convert enum to string for display
                    ServiceStatus = c.ServiceStatus.ToString(), // Convert enum to string for display
                  


                }).ToList();


            // Check if gridview needs to be cleared (can cause flicker)
            // A better approach is to set binding source's datasource

            gridviewServiceTaskViewer.DataSource = servicerViewModels;

        }
    }
}
