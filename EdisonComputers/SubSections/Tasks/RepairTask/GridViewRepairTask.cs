using BLL.Services.DeviceService;
using BLL.Services.RepairRecordService;
using Core.IRepository;
using Core.Models;
using Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Sections.Customers;
using Presentation.SubSections.Customers;
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

namespace Presentation.SubSections.Tasks.RepairTask
{
    public partial class GridViewRepairTask : UserControl
    {
        private IServiceProvider serviceProvider;
        private int jobId;
        private Action showAddRepairTaskView;
        private readonly Action<int> _showEditRepairTaskViewCallback; //calback


        public GridViewRepairTask(IServiceProvider serviceProvider, int jobId, Action showAddRepairTaskView, Action<int> showEditRepairTaskView)
        {

            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.jobId = jobId;
            this.showAddRepairTaskView = showAddRepairTaskView;
            _showEditRepairTaskViewCallback = showEditRepairTaskView;
           
        }

        private async void GridViewRepairTask_Load(object sender, EventArgs e)
        {
            SetupGridViewColumns(); // Set up columns only once
            await LoadAllRepairTasksAsync(); // Load all repair tasks when the control loads   



        }

        //private async Task LoadRepairTasks()
        //{
        //    try

        //    {

                


        //        var serviceScope = serviceProvider.CreateScope();
        //        var repairrecordService = serviceScope.ServiceProvider.GetRequiredService<RepairRecordService>();


        //        var repairTasks = await repairrecordService.GetRepairRecordsByJobIdAsync(jobId);

        //        gridviewRepairTaskViewer.DataSource = repairTasks
        //            .Where(rt => rt.JobId == jobId) // Filter tasks by jobId
        //            .Select(rt => new
        //            {
        //                rt.RepairRecordId,
        //                rt.ProblemDescription,
        //                rt.RepairNotes,
        //                RepairStatus = rt.RepairStatus.ToString(), // Convert enum to string for display
        //                WarrantyExpiry = rt.WarrantyExpiryDate?.ToString("yyyy-MM-dd") ?? "N/A"

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
            if (gridviewRepairTaskViewer.Columns.Contains("View")) return; // prevent duplication



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


            gridviewRepairTaskViewer.Columns.Add(editColumn);
            gridviewRepairTaskViewer.Columns.Add(deleteColumn);
        }



        private void SetupGridViewColumns()
        {
            // Call this once,  in the UserControl's constructor or Load event.
            // Ensure it only runs once.
            if (gridviewRepairTaskViewer.Columns.Count > 0) return;

            gridviewRepairTaskViewer.AutoGenerateColumns = false; // Turn off auto-generation
            gridviewRepairTaskViewer.Columns.Clear();

            // Define  data columns MANUALLY
            gridviewRepairTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RepairRecordId",
                HeaderText = "RepairRecordId",
                DataPropertyName = "RepairRecordId", // Must match property name in CustomerViewModel
                Visible = false // Often hide the ID
            });
            gridviewRepairTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            gridviewRepairTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RepairStatus",
                HeaderText = "Repair Status",
                DataPropertyName = "RepairStatus",
                Width = 200
            });
            gridviewRepairTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Notes",
                HeaderText = "Notes",
                DataPropertyName = "Notes",
                Width = 120
            });
            gridviewRepairTaskViewer.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WarrantyExpiryDate",
                HeaderText = "Warranty Expiry Date",
                DataPropertyName = "WarrantyExpiryDate",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "MM/dd/yyyy" } // Format date

            });




            // NOW add  action columns
            AddActionColumns();
        }







        //handle cell click events to perform actions based on the clicked cell
        private async void gridviewRepairTaskViewer_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Ignore header row
            // Ensure the clicked cell is one of the action columns
            string columnName = gridviewRepairTaskViewer.Columns[e.ColumnIndex].Name;

            var repairtaskId = (int)gridviewRepairTaskViewer.Rows[e.RowIndex].Cells["RepairRecordId"].Value;


            if(columnName == "Edit")
            {
                // Call the callback to show the edit view
                _showEditRepairTaskViewCallback(repairtaskId);
            }
            else if (columnName == "Delete")
            {
                // Handle delete action
                var confirmResult = MessageBox.Show("Are you sure you want to Cancel this repair task?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    //change the status of the repair task to cancelled
                    var serviceScope = serviceProvider.CreateScope();
                    var genericService = serviceScope.ServiceProvider.GetRequiredService<IRepository<RepairRecord>>();
                    var repairTask = await genericService.GetById(repairtaskId);
                    if (repairTask != null)
                    {
                        repairTask.RepairStatus = Core.Enums.RepairStatus.Cancelled; // Assuming you have a Cancelled status
                        await genericService.Update(repairTask);
                        MessageBox.Show("Repair task cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //refresh the grid view by calling the callback method
                        await RefreshGridView();
                    }
                    else
                    {
                        MessageBox.Show("Repair task not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        //refresh the grid view
        public async Task RefreshGridView()
        {
            await LoadAllRepairTasksAsync(); // Reload the repair tasks
        }






        private async Task LoadAllRepairTasksAsync()
        {
            try
            {
                using (var scope = this.serviceProvider.CreateScope())
                {
                    var repairrecordService = scope.ServiceProvider.GetRequiredService<RepairRecordService>();
                    var repairrecords = await repairrecordService.GetRepairRecordsByJobIdAsync(jobId);
                    // Note: GetAllCustomers returns IEnumerable, so need ToList()
                    BindRepairTasksToGrid(repairrecords.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Repair Tasks: {ex.Message}", "Error");
            }
        }




        private void BindRepairTasksToGrid(List<RepairRecord> repairRecords)
        {
            var repairViewModels = repairRecords
                .Where(rr=> rr.JobId == jobId)
                .Select(c => new RepairRecordViewModel
            {
                RepairRecordId = c.RepairRecordId,
                Description = c.ProblemDescription,
                Notes = c.RepairNotes,
                RepairStatus = c.RepairStatus.ToString(), //RepairStatus is an enum
                                                          //show the warranty expiry date as a string
                WarrantyExpiryDate = c.WarrantyExpiryDate?.ToString("MM/dd/yyyy") ?? "No Warranty"


                    //WarrantyExpiryDate = c.WarrantyPeriodinMonths.ToString()

                }).ToList();
            

            // Check if gridview needs to be cleared (can cause flicker)
            // A better approach is to set binding source's datasource

            gridviewRepairTaskViewer.DataSource = repairViewModels;

        }










    }
}
