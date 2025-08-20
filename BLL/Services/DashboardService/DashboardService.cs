using Core.Enums;
using Core.IRepository.IComponentRepository;
using Core.IRepository.ICustomerRepository;
using Core.IRepository.IDeviceRepository;
using Core.IRepository.IJobRepository;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.DashboardService
{

    public class DashboardService
    {


        private readonly IjobRepository _jobRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IComponentRepository _componentRepository;


        public DashboardService(IjobRepository ijobRepository, ICustomerRepository customerRepository, IDeviceRepository deviceRepository,IComponentRepository componentRepository) 
        {
            _jobRepository = ijobRepository;
            _customerRepository = customerRepository;
            _deviceRepository = deviceRepository;
            _componentRepository = componentRepository;


        }
        public async Task<int> ActiveJobCountAsync()
        {
            // Active jobs: InProgress, AwaitingParts, AwaitingCustomerApproval, etc.
            // NOT CheckedIn (maybe), NOT TasksCompleted, ReadyForBilling, ReadyForPickup, Collected, Cancelled

            var activeStatuses = new List<Core.Enums.JobStatus>
            {
                Core.Enums.JobStatus.InProgress,
                Core.Enums.JobStatus.AwaitingParts,
                Core.Enums.JobStatus.CheckedIn
            };

            return (await _jobRepository.CountJobsByStatusesAsync(activeStatuses));

        }

        public async Task<int> CountJobsByStatusAndBillingAsync()
        {
            // Count jobs by status and billing information
            return await _jobRepository.CountJobsByStatusAndBillingAsync();
        }

        public async Task<int> CountCompletedJobsNotPaidAsync()
        {
            // Count completed jobs that are not paid
            return await _jobRepository.CountCompletedJobsNotPaidAsync();
        }


        public async Task<int> GetNewCustomersThisMonthCountAsync()
        {
            // Get the count of new customers added this month
            return await _customerRepository.GetNewCustomersThisMonthCountAsync();
        }


        //a method to return recent activities as a RecentActivityViewModel list
        public async Task<List<RecentActivityViewModel>> GetRecentActivitiesAsync(int count = 5)
        {
           var recentJobs = await _jobRepository.GetRecentJobsAsync(count);

            //create a list of RecentActivityViewModel 

            var recentActovities = new List<RecentActivityViewModel>();

            foreach(var job in recentJobs)
            {
                recentActovities.Add(new RecentActivityViewModel
                {
                    Timestamp = job.CheckedInDate,
                    Description = $" New Job: {job.JobId} logged for {job.Device?.Model ?? "N/A"  } {job.Device?.SerialNumber} - {job.Device?.Customer?.FirstName ?? "N/A"}",
                   
                });

                //also add the recent payment activities 
                if(job.Billing != null && job.Billing.IsPaid == true && job.Billing.PaymentDate.HasValue && job.Billing.PaymentDate.Value > DateTime.UtcNow.AddDays(-5))//last 5 days
                {
                    recentActovities.Add(new RecentActivityViewModel
                    {
                        Timestamp = job.Billing.PaymentDate.Value,
                        Description = $"Payment received for Job: #{job.JobId} - Amount: {job.Billing.Amount} with Customer Name: {job.Device?.Customer?.LastName} ",
                        //IconClass = "fas fa-money-bill-wave", // Example icon class
                        //IconColor = "#27ae60" // Example color
                    });
                }

            }
            //sort by timestamp descending and take the top count
            return recentActovities.OrderByDescending(x => x.Timestamp).Take(count).ToList();
        }


        //a method to get the monthly summary for a given year
        public async Task<List<MonthlySummaryViewModel>> GetMonthlySummaryAsync(int year)
        {
            return await _jobRepository.GetMonthlySummaryAsync(year);
        }



        //a method to get jobs needing attention
        public async Task<List<JobAttentionViewModel>> GetJobsNeedingAttentionAsync(int limit = 5)
        {
                // Define which statuses require attention
                var attentionStatuses = new List<JobStatus>
            {
                JobStatus.CheckedIn,
                JobStatus.AwaitingParts,
                JobStatus.ReadyForPickup
                //JobStatus.CheckedIn
            };

            return await _jobRepository.GetJobsNeedingAttentionAsync(attentionStatuses, limit);
        }



        //a method to get components that are low in stock
        public async Task<List<LowinStockComponentsViewModel>> GetLowStockComponentsAsync(int threshold = 5)
        {
            // Get components that are low in stock (e.g., less than 5 units)
            return await _componentRepository.GetLowStockComponentsAsync(threshold);
        }



    }
}
