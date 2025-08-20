using BLL.Services.DeviceService;
using BLL.Services.JobService;
using Core.Enums;
using Core.Models;
using Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Sections.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.SubSections.Jobs
{
    public partial class JobGridViewSection : UserControl
    {
        private readonly IServiceProvider _serviceProvider; // service provider to resolve dependencies
        private readonly JobsSection _jobsSection;
        private readonly Action _callback; // Callback to switch back to the jobs section if needed


        public JobGridViewSection(IServiceProvider serviceProvider, JobsSection jobsSection)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _jobsSection = jobsSection;


        }

        private async void JobGridViewSection_Load(object sender, EventArgs e)
        {
            SetupGridViewColumns();
            PopulateFilterCategories(); // Populate the filter categories

            //load the default jobs data
            //await LoadJobsData(); // Load recent jobs by default


            await LoadRecentJobsByDefaultAsync(); // Load recent jobs by default

        }


        private void PopulateFilterCategories()
        {
            var categories = new List<FilterCategory>
            {
                new FilterCategory { DisplayName = "Recent Jobs (15 Days)", Value = "RecentDefault" }, // New default
                new FilterCategory { DisplayName = "Show All", Value = "All" },
                new FilterCategory { DisplayName = "By Job Status", Value = "JobStatus" },
                //new FilterCategory { DisplayName = "By Billing Status", Value = "BillingStatus" }, // ADD THIS
                //new FilterCategory { DisplayName = "By Date Range", Value = "RecentJobs" } // Renamed for clarity
            };

            cmbboxjobsectionfilterCategory.DataSource = categories;
            cmbboxjobsectionfilterCategory.DisplayMember = "DisplayName";
            cmbboxjobsectionfilterCategory.ValueMember = "Value";

            // Wire up the event handler for when the user changes the filter category
            cmbboxjobsectionfilterCategory.SelectedIndexChanged += cmbboxjobsectionfilterCategory_SelectedIndexChanged_1;
        }



        public async Task RefreshGridView()
        {
            // The logic is simpler: find out the current filter and re-apply it.
            if (cmbboxjobsectionfilterCategory.SelectedValue != null)
            {
                string category = cmbboxjobsectionfilterCategory.SelectedValue.ToString();

                if (category == "All")
                {
                    await LoadAllJobsAsync();
                }
                else if (category == "RecentDefault")
                {
                    await LoadRecentJobsByDefaultAsync();
                }
                else if (cmbboxjobsectionfilterValue.SelectedValue != null)
                {
                    // If another filter is active, just re-trigger its event handler
                    cmbboxjobsectionfilterValue_SelectedIndexChanged_1(this, EventArgs.Empty);
                }
                else
                {
                    // Fallback to the default if no specific value is selected
                    await LoadRecentJobsByDefaultAsync();
                }
            }
            else
            {
                // Fallback if even the category isn't set
                await LoadRecentJobsByDefaultAsync();
            }
        }









        //public async Task LoadJobsData()
        //{
        //    try
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
        //            var jobs = await jobService.GetAllJobs();
        //            // Convert to view model if necessary

        //            var jobVMs = jobs.Select(j => new Core.ViewModels.JobViewModel
        //            {
        //                JobId = j.JobId,
        //                Description = j.JobDescription,
        //                JobStatus = j.JobStatus.ToString(),
        //                CreatedDate = j.CheckedInDate,
        //                ManualLaborCost = (decimal)j.ManualJobCost,
        //            }).ToList();

        //            jobgridView.DataSource = jobVMs;

        //            // Add action columns to the grid view
        //            AddActionColumns();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error loading jobs data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        //add action columns to the grid view
        private void AddActionColumns()
        {
            if (jobgridView.Columns.Contains("View")) return; // prevent duplication
            DataGridViewImageColumn viewColumn = new DataGridViewImageColumn
            {
                Name = "View",
                HeaderText = "View",



                Image = Resource1.Eye2, // 👁️
                Width = 2,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
            };
            //DataGridViewImageColumn editColumn = new DataGridViewImageColumn
            //{
            //    Name = "Edit",
            //    HeaderText = "Edit",

            //    Image = Resource1.Create, // ✏️
            //    Width = 2,
            //    ImageLayout = DataGridViewImageCellLayout.Zoom

            //};
            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn
            {
                Name = "Delete",
                HeaderText = "Delete",
                Image = Resource1.Waste, // 🗑️
                Width = 2,
                ImageLayout = DataGridViewImageCellLayout.Zoom

            };
            jobgridView.Columns.Add(viewColumn);
            //jobgridView.Columns.Add(editColumn);
            jobgridView.Columns.Add(deleteColumn);
        }




        private void SetupGridViewColumns()
        {
            // Call this once,  in the UserControl's constructor or Load event.
            // Ensure it only runs once.
            if (jobgridView.Columns.Count > 0) return;

            jobgridView.AutoGenerateColumns = false; // Turn off auto-generation
            jobgridView.Columns.Clear();

            // Define  data columns MANUALLY
            jobgridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "JobId",
                HeaderText = "Job Id",
                DataPropertyName = "JobId", // Must match property name in CustomerViewModel
                Visible = false // Often hide the ID
            });
            jobgridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            jobgridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CustomerName",
                HeaderText = "Customer Name",
                DataPropertyName = "CustomerName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            jobgridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Device",
                HeaderText = "Device",
                DataPropertyName = "Device",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            jobgridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "JobStatus",
                HeaderText = "Job Status",
                DataPropertyName = "JobStatus",
                Width = 200
            });
            jobgridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Created Date",
                HeaderText = "Created Date",
                DataPropertyName = "CreatedDate",
                Width = 120
            });

            jobgridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ManualLaborCost",
                HeaderText = "Labor Cost",
                DataPropertyName = "ManualLaborCost",
                Width = 200
            });



            // NOW add  action columns
            AddActionColumns();
        }









        private async void jobgridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //handle cell click events to perform actions based on the clicked cell

            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Ignore header clicks

            //ensure the clicked cell is an action column

            string columnName = jobgridView.Columns[e.ColumnIndex].Name;

            //get the job id from the row data

            int jobId = (int)jobgridView.Rows[e.RowIndex].Cells["JobId"].Value;


            switch (columnName)
            {
                case "View":
                    // Handle view action
                    var viewJobSection = new ViewJobSection(_serviceProvider, jobId, _jobsSection.RefreshAndShowGrid); //changed from this ()=> _jobsSection.SwitchJobSection(this) to _jobsSection.RefreshAndShowGrid and removed the callback parameter from ViewJobSection constructor. this is because we want to switch back to the job grid view after viewing the job details
                    _jobsSection.SwitchJobSection(viewJobSection);
                    break;
                case "Edit":
                // Handle edit action
                //var editJobSection = _serviceProvider.GetRequiredService<EditJobSection>();
                //editJobSection.LoadJobForEditing(jobId);
                //_jobsSection.SwitchJobSection(editJobSection);
                //break;
                case "Delete":
                    // Handle delete action
                    var confirmResult = MessageBox.Show("Are you sure you want to cancel this job?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirmResult == DialogResult.Yes)
                    {
                        //without deleting the job, we can just remove it from the grid view and make the job status to cancelled
                        //get the job and set its job status to cancelled
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
                            try
                            {
                                // Call the service to cancel the job
                                await jobService.CancelJobAsync(jobId);
                                // Optionally, refresh the grid view after deletion
                                //_callback?.Invoke(); // Call the callback to refresh the grid view
                                //await LoadJobsData();
                                // Or you can call the RefreshGridView method directly
                                await RefreshGridView(); // Refresh the grid view to reflect changes
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error cancelling job: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }


                    }
                    break;
            }


        }

        //private async void cmbboxjobsectionfilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        //private async void cmbboxjobsectionfilterValue_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //}



        // method for the default load
        private async Task LoadRecentJobsByDefaultAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
                    // Directly call the method to get jobs from the last 15 days
                    var jobs = await jobService.GetRecentJobsAsync(15);
                    BindJobsToGrid(jobs.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading recent jobs: {ex.Message}", "Error");
            }
        }




        // Method to load all jobs, used when the user selects "Show All"
        private async Task LoadAllJobsAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<JobService>();

                    //if user enter search text, filter the jobs by search text
                    if (!string.IsNullOrWhiteSpace(searchboxjobsgriviewSection.Text))
                    {
                        var searchText = searchboxjobsgriviewSection.Text.Trim();
                        var searchedjobs = await jobService.SearchJobsByDeviceAndCustomerAsync(searchText);
                        BindJobsToGrid(searchedjobs.ToList());
                        return; // Exit after filtering
                    }


                    var jobs = await jobService.GetAllJobsWithDeviceAndCustomerAsync();
                    // Note: GetAllCustomers returns IEnumerable, so need ToList()
                    BindJobsToGrid(jobs.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Jobs: {ex.Message}", "Error");
            }
        }

        private void BindJobsToGrid(List<Job> jobs)
        {
            var jobViewModels = jobs.Select(c => new JobViewModel
            {
                JobId = c.JobId,
                Description = c.JobDescription,
                CustomerName = c.Device?.Customer?.FullName ?? "Unknown", // Assuming Device has a Customer property
                Device = c.Device?.Model ?? "Unknown", // Assuming Device has a DeviceName property
                JobStatus = c.JobStatus.ToString(),
                CreatedDate = c.CheckedInDate,
                ManualLaborCost = c.ManualJobCost ?? 0.0m, // Handle nullable decimal



                // Assuming DeviceImages is a collection of images, you might want to handle it differently
                // UploadedImages = c.DeviceImages.Select(img => new DeviceImageViewModel { ImagePath = img.ImagePath }).ToList()   


            }).ToList();

            // Check if gridview needs to be cleared (can cause flicker)
            // A better approach is to set binding source's datasource

            jobgridView.DataSource = jobViewModels;

        }

        private async void cmbboxjobsectionfilterCategory_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbboxjobsectionfilterCategory.SelectedValue == null) return;

            string selectedCategory = cmbboxjobsectionfilterCategory.SelectedValue.ToString();

            // Clear the second ComboBox before populating
            cmbboxjobsectionfilterValue.DataSource = null;
            cmbboxjobsectionfilterValue.Enabled = true;
            cmbboxjobsectionfilterValue.DropDownStyle = ComboBoxStyle.DropDownList; // Good for enums/predefined lists

            switch (selectedCategory)
            {
                case "JobStatus":
                    cmbboxjobsectionfilterValue.DataSource = Enum.GetValues(typeof(Core.Enums.JobStatus));
                    break;




                //case "BillingStatus": // ADD THIS CASE
                //                      // We can just use a simple list of strings or a custom class for this
                //    cmbboxjobsectionfilterValue.DataSource = new[] {
                //        new { Display = "Paid", Value = true },
                //        new { Display = "Unpaid", Value = false }
                //    };
                //    cmbboxjobsectionfilterValue.DisplayMember = "Display";
                //    cmbboxjobsectionfilterValue.ValueMember = "Value";
                //    break;



                //case "RecentJobs":
                //    // Use a descriptive list for the user
                //    cmbboxjobsectionfilterValue.DataSource = new[] {
                //    new { Display = "Last 5 Days", Value = 5 },
                //    new { Display = "Last 10 Days", Value = 10 },
                //    new { Display = "Last 15 Days", Value = 15 },
                //    new { Display = "Last 30 Days", Value = 30 }
                //    };
                //    cmbboxjobsectionfilterValue.DisplayMember = "Display";
                //    cmbboxjobsectionfilterValue.ValueMember = "Value";
                //    break;

                case "RecentDefault": // Handle the new default view


                case "All":
                default:
                    cmbboxjobsectionfilterValue.Enabled = false;
                    // The actual data loading will be triggered by the value combo box event,
                    // or we can trigger it directly for "All"
                    if (selectedCategory == "All") await LoadAllJobsAsync();
                    if (selectedCategory == "RecentDefault") await LoadRecentJobsByDefaultAsync();
                    break;
            }


        }

        private async void cmbboxjobsectionfilterValue_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            if (cmbboxjobsectionfilterCategory.SelectedItem == null || cmbboxjobsectionfilterValue.SelectedValue == null) return;

            string category = cmbboxjobsectionfilterCategory.SelectedValue.ToString();
            object value = cmbboxjobsectionfilterValue.SelectedValue;

            if (value == null) return;



            List<Job> filteredJobs = new List<Job>();

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
                    switch (category)
                    {
                        case "JobStatus":
                            filteredJobs = await jobService.GetJobsByStatusAsync((Core.Enums.JobStatus)cmbboxjobsectionfilterValue.SelectedItem);
                            break;


                            //case "BillingStatus":
                            //    bool isPaid = (bool)((dynamic)value).Value; // Cast to dynamic to access Value property
                            //    filteredJobs = await jobService.GetAllJobsWithBillingAsync(isPaid);
                            //    break;


                            //case "RecentJobs":
                            //    //provide the number of days as an integer value
                            //    int days = (int)value;
                            //    filteredJobs = await jobService.GetRecentJobsAsync(days);
                            //    break;
                    }
                }
                BindJobsToGrid(filteredJobs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filter: {ex.Message}", "Error");
            }

        }

        private void searchboxjobsgriviewSection_TextChanged(object sender, EventArgs e)
        {
            _ = LoadAllJobsAsync(); // Call the method to filter jobs based on search text

        }
    }
}
