using BLL.Services.DashboardService;
using Core.IRepository;
using Core.IRepository.ICustomerRepository;
using Core.IRepository.IDeviceRepository;
using Core.IRepository.IJobRepository;
using Core.ViewModels;
using EdisonComputers;
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

namespace Presentation.Sections.Dashboard
{
    public partial class DashboardSection : UserControl
    {
        private readonly IServiceProvider _serviceProvier;
        private readonly JobsSection _jobsSection; // Assuming you need this for some reason, otherwise you can remove it
        public DashboardSection(IServiceProvider serviceProvider, JobsSection jobsSection)
        {
            InitializeComponent();
            _serviceProvier = serviceProvider;


            //wire up the event handlers for the year ComboBox and toggle switch
            dashboardsectioncmbboxChartYear.SelectedIndexChanged += ChartFilter_Changed;
            toggleProfit.CheckedChanged += ChartFilter_Changed;
            _jobsSection = jobsSection;
        }

        private async void DashboardSection_Load(object sender, EventArgs e)
        {


            // Populate year ComboBox
            for (int i = 0; i < 5; i++)
            {
                dashboardsectioncmbboxChartYear.Items.Add(DateTime.Now.Year - i);
            }
            dashboardsectioncmbboxChartYear.SelectedIndex = 0;

            await LoadDashboardDataAsync();
        }

        public async Task LoadDashboardDataAsync()
        {
            // Get the DashboardService from the service provider

            try
            {
                using (var scope = _serviceProvier.CreateScope())
                {

                    var dashboardService = scope.ServiceProvider.GetRequiredService<BLL.Services.DashboardService.DashboardService>();

                    //load card data
                    int activeJobCount = await dashboardService.ActiveJobCountAsync();
                    lblDashboardsectionactiveJobsCount.Text = activeJobCount.ToString();

                    int readyforPickupCount = await dashboardService.CountJobsByStatusAndBillingAsync();
                    lblDashboardSectionRFPcount.Text = readyforPickupCount.ToString();

                    int notyetpaid = await dashboardService.CountCompletedJobsNotPaidAsync();
                    lbldashboardSectionNotPaidCount.Text = notyetpaid.ToString();


                    int customersthisMonth = await dashboardService.GetNewCustomersThisMonthCountAsync();
                    lblDashboardSectionNewCustomersCount1.Text = customersthisMonth.ToString();


                    //load jobs needing attention
                    var jobsNeedingAttention = await dashboardService.GetJobsNeedingAttentionAsync(5);

                    dgvJobsNeedingAttention.DataSource = jobsNeedingAttention;

                    FormatAttentionGrid(); // A helper to style the grid

                    //load low stock components
                    var lowStockComponents = await dashboardService.GetLowStockComponentsAsync(5);
                    dgvlowstockComponents.DataSource = lowStockComponents;

                    FormatLowStockGrid(); // A helper to style the grid



                    //call the method to update the chart
                    await UpdateChartAsync();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading dashboard data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }





        private async Task UpdateChartAsync()
        {
            if (dashboardsectioncmbboxChartYear.SelectedItem == null) return;
            int selectedYear = (int)dashboardsectioncmbboxChartYear.SelectedItem;

            // --- 1. Fetch Data (This part is correct) ---
            List<MonthlySummaryViewModel> summaryData;
            using (var scope = _serviceProvier.CreateScope())
            {
                var dashboardService = scope.ServiceProvider.GetRequiredService<DashboardService>();
                summaryData = await dashboardService.GetMonthlySummaryAsync(selectedYear);
            }

            // --- 2. Clear Previous Chart Data ---
            chartMonthlySummary.Datasets.Clear(); // Use Datasets collection
            chartMonthlySummary.YAxes.GridLines.Display = true; // Optional: ensure grid lines are visible

            // --- 3. Prepare Chart Data and Series ---
            var series = new Guna.Charts.WinForms.GunaBarDataset(); // Use the correct class

            // Check which toggle is on
            bool showProfit = toggleProfit.Checked;

            series.Label = showProfit ? "Monthly Profit(LKR)" : "Monthly Sales(LKR)";
            // Fix for CS0029: Cannot implicitly convert type 'System.Drawing.Color' to 'Guna.Charts.WinForms.ColorCollection'

            // The `FillColors` property expects a `ColorCollection` type, not a single `Color`. 
            // To fix this, create a `ColorCollection` and add the desired color to it.

            series.FillColors = new Guna.Charts.WinForms.ColorCollection
            {
                ColorTranslator.FromHtml("#D81E2C") // Your theme's red
            };
            series.BorderColors = new Guna.Charts.WinForms.ColorCollection
            {
                ColorTranslator.FromHtml("#a71722") // Hover color
            };


            // --- 4. Prepare Month Labels ---
            var allMonths = Enumerable.Range(1, 12)
                .Select(m => new DateTime(selectedYear, m, 1).ToString("MMM"))
                .ToList();

            // --- 5. Prepare Data Points ---
            var chartDataPoints = new List<Guna.Charts.WinForms.LPoint>();



            // Populate the data points
            for (int i = 0; i < 12; i++)
            {
                var monthData = summaryData.FirstOrDefault(m => m.Month == i + 1);

                double value = 0;
                if (monthData != null)
                {
                    value = showProfit ? (double)monthData.TotalProfit : (double)monthData.TotalSales;
                }

                chartDataPoints.Add(new Guna.Charts.WinForms.LPoint
                {
                    Label = allMonths[i],
                    Y = value < 0 ? 0 : value
                });
            }

            // --- 6. Add Data to Series ---
            series.DataPoints.AddRange(chartDataPoints.ToArray());
            chartMonthlySummary.Datasets.Add(series);

            // --- 7. Remove the problematic Labels line ---
            // chartMonthlySummary.Labels = allMonths.ToArray(); // This line causes the error

            // --- 8. Redraw the Chart ---
            chartMonthlySummary.Update();
        }

        // Event handler for when the user changes the year or toggles the switch
        private async void ChartFilter_Changed(object sender, EventArgs e)
        {
            // Add debugging
            bool showProfit = toggleProfit.Checked;
            System.Diagnostics.Debug.WriteLine($"Toggle changed: showProfit = {showProfit}");

            await UpdateChartAsync();
        }




        // a formatting method for the DataGridView displaying jobs needing attention
        private void FormatAttentionGrid()
        {
            dgvJobsNeedingAttention.AutoGenerateColumns = true; // Let it auto-generate for simplicity
            dgvJobsNeedingAttention.Columns["JobId"].HeaderText = "Job ID";
            dgvJobsNeedingAttention.Columns["CustomerName"].HeaderText = "Customer";
            dgvJobsNeedingAttention.Columns["DeviceIdentifier"].HeaderText = "Device";
            dgvJobsNeedingAttention.Columns["DeviceIdentifier"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvJobsNeedingAttention.Columns["DaysOpen"].HeaderText = "Days Open";
            dgvJobsNeedingAttention.Columns["DaysOpen"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Optionally add a "View Job" button column
        }



        //a formatting method for the lowinstock components DataGridView
        private void FormatLowStockGrid()
        {
            dgvlowstockComponents.AutoGenerateColumns = true; // Let it auto-generate for simplicity
            dgvlowstockComponents.Columns["ComponentId"].HeaderText = "Component ID";
            dgvlowstockComponents.Columns["SerialNumber"].HeaderText = "Serial Number";
            dgvlowstockComponents.Columns["ComponentName"].HeaderText = "Component Name";

            dgvlowstockComponents.Columns["ComponentType"].HeaderText = "Type";
            dgvlowstockComponents.Columns["ComponentType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvlowstockComponents.Columns["QuantityInStock"].HeaderText = "Quantity in Stock";
            dgvlowstockComponents.Columns["QuantityInStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;




        }
        // Event handler for when the user clicks on a cell in the jobs needing attention DataGridView
        private void dgvJobsNeedingAttention_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           



        }
        //show a tooltip when the user hovers over a cell in the jobs needing attention DataGridView
        private void dgvJobsNeedingAttention_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = dgvJobsNeedingAttention.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value != null)
                {
                    // Show tooltip with the cell value
                    tooltip1.SetToolTip(dgvJobsNeedingAttention, cell.Value.ToString());
                }
            }

        }
    }
}
