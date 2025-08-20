using BLL.Services.RepairRecordService;
using BLL.Services.UpgradeRecordService;
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

namespace Presentation.SubSections.Tasks.UpgradeTask
{
    public partial class GridViewUpgradeTask : UserControl
    {
        private readonly IServiceProvider serviceProvider;
        private int jobId;
        private Action showAddUpgradeTaskView;
        private Action<int> showEditUpgradeTaskView;//callback for edit action





        public GridViewUpgradeTask(IServiceProvider serviceProvider, int jobId, Action showAddUpgradeTaskView, Action<int> showEditUpgradeTaskView)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.jobId = jobId;
            this.showAddUpgradeTaskView = showAddUpgradeTaskView;
            this.showEditUpgradeTaskView = showEditUpgradeTaskView;
        }

        private async void GridViewUpgradeTask_Load(object sender, EventArgs e)
        {
            // Load the upgrade tasks for the specified jobId
            SetupGridViewColumns();
            await LoadAllUpgradeTasksAsync(); // Load tasks asynchronously

        }

        //private async Task LoadUpgradeTasksAsync()
        //{
        //    try
        //    {
        //        var serviceScope = serviceProvider.CreateScope();
        //        var upgradeRecordService = serviceScope.ServiceProvider.GetRequiredService<IRepository<UpgradeRecord>>();
        //        var upgradeTasks = await upgradeRecordService.GetAll();
        //        gridviewUpgradeTaskViewer.DataSource = upgradeTasks
        //            .Where(ut => ut.JobId == jobId) // Filter tasks by jobId
        //            .Select(ut => new
        //            {
        //                ut.UpgradeRecordId,
        //                ut.UpgradeDescription,

        //                UpgradeType = ut.UpgradeType.ToString(), // Convert enum to string for display
        //                UpgradeStatus = ut.UpgradeStatus.ToString(), // Convert enum to string for display



        //            })
        //            .ToList();

        //        AddActionColumns(); // Add action columns after setting the data source
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred while loading upgrade tasks: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}



        private void AddActionColumns()
        {
            if (gridviewUpgradeTaskViewer.Columns.Contains("View")) return; // prevent duplication



            DataGridViewImageColumn editColumn = new DataGridViewImageColumn
            {
                Name = "Edit",
                HeaderText = "Edit",
                Image = Resource1.Create, // ✏️
                Width = 2,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };

            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn
            {
                Name = "Delete",
                HeaderText = "Delete",
                Image = Resource1.Waste, // 🗑️
                Width = 2,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };


            gridviewUpgradeTaskViewer.Columns.Add(editColumn);
            gridviewUpgradeTaskViewer.Columns.Add(deleteColumn);
        }



        private void SetupGridViewColumns()
        {
            // Call this once,  in the UserControl's constructor or Load event.
            // Ensure it only runs once.
            if (gridviewUpgradeTaskViewer.Columns.Count > 0) return;

            gridviewUpgradeTaskViewer.AutoGenerateColumns = false; // Turn off auto-generation
            gridviewUpgradeTaskViewer.Columns.Clear();

            // Define  data columns MANUALLY
            gridviewUpgradeTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UpgradeRecordId",
                HeaderText = "Upgrade Record ID",
                DataPropertyName = "UpgradeRecordId", // Must match property name in CustomerViewModel
                Visible = false // Often hide the ID
            });
            gridviewUpgradeTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UpgradeDescription",
                HeaderText = "Description",
                DataPropertyName = "UpgradeDescription",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Width = 200
            });
            gridviewUpgradeTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UpgradeType",
                HeaderText = "Upgrade Type",
                DataPropertyName = "UpgradeType",
                Width = 200
            });
            gridviewUpgradeTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UpgradeStatus",
                HeaderText = "Upgrade Status",
                DataPropertyName = "UpgradeStatus",
                Width = 120
            });

            // NOW add  action columns
            AddActionColumns();
        }










        private async void gridviewUpgradeTaskViewer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Ignore header row
            // Ensure the clicked cell is one of the action columns
            string columnName = gridviewUpgradeTaskViewer.Columns[e.ColumnIndex].Name;

            var ugradetaskId = (int)gridviewUpgradeTaskViewer.Rows[e.RowIndex].Cells["UpgradeRecordId"].Value;


            if (columnName == "Edit")
            {
                // Call the callback to show the edit view
                showEditUpgradeTaskView(ugradetaskId);
            }
            else if (columnName == "Delete")
            {
                // Handle delete action
                var confirmResult = MessageBox.Show("Are you sure you want to Cancel this update task?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    //change the status of the repair task to cancelled
                    var serviceScope = serviceProvider.CreateScope();
                    var genericService = serviceScope.ServiceProvider.GetRequiredService<IRepository<UpgradeRecord>>();
                    var upgradeTask = await genericService.GetById(ugradetaskId);
                    if (upgradeTask != null)
                    {
                        upgradeTask.UpgradeStatus = Core.Enums.UpgradeStatus.Cancelled; // Assuming you have a Cancelled status
                        await genericService.Update(upgradeTask);
                        MessageBox.Show("Upgrade task cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshGridView(); // Refresh the grid view
                    }
                    else
                    {
                        MessageBox.Show("Upgrade task not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }


        //public call back method to refresh the grid view when a new upgrade task is added or edited
        public async Task RefreshGridView()
        {
            try
            {
                await LoadAllUpgradeTasksAsync(); // Reload the upgrade tasks
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing upgrade tasks: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private async Task LoadAllUpgradeTasksAsync()
        {
            try
            {
                using (var scope = this.serviceProvider.CreateScope())
                {
                    var upgraderecordService = scope.ServiceProvider.GetRequiredService<UpgradeRecordService>();
                    var upgradeRecords = await upgraderecordService.GetUpgradeRecordsByJobIdAsync(jobId);
                    // Note: GetAllCustomers returns IEnumerable, so need ToList()
                    BindUpgradeTasksToGrid(upgradeRecords.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Devices: {ex.Message}", "Error");
            }
        }




        private void BindUpgradeTasksToGrid(List<UpgradeRecord> upgradeRecords)
        {


            SetupGridViewColumns(); // Ensure columns are set up before binding data
            var upgradeViewModels = upgradeRecords
                .Where(ur => ur.JobId == jobId)
                .Select(c => new UpgradeRecordViewModel
                {
                    UpgradeRecordId = c.UpgradeRecordId,
                    UpgradeDescription = c.UpgradeDescription,
                    UpgradeType = c.UpgradeType.ToString(), // Convert enum to string for display
                    UpgradeStatus = c.UpgradeStatus.ToString(), // Convert enum to string for display



                }).ToList();


            // Check if gridview needs to be cleared (can cause flicker)
            // A better approach is to set binding source's datasource

            gridviewUpgradeTaskViewer.DataSource = upgradeViewModels;

        }

    }
}
