using Core.Enums;
using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.IJobRepository
{
    public interface IjobRepository : IRepository<Job>
    {
        // Define any additional methods specific to job repository here
        // For example, methods to get jobs by status, priority, etc.
        Task<List<Job>> GetJobsByStatusAsync(JobStatus status);
        // Task<IEnumerable<Job>> GetJobsByPriorityAsync(JobPriority priority);
        Task<IEnumerable<Job>> GetJobsByDeviceIdAsync(int deviceId);
        Task<Job> AddJob( int deviceId, Job job);
        Task<Job> UpdateJob(int jobId, Job jobToUpdate);
        //get job by id with device customer and billing information
        //Task <Job> GetJobByIdWithDetailsAsync(int jobId);

        Task<Job> GetJobByIdWithAllDetailsForBillingAsync(int jobId);


        //Task SaveOrUpdateJobBillingAsync(
        //int jobId, decimal billingAmount, decimal manualJobCost,
        //bool isPaid, DateTime? paymentDate, Core.Enums.PaymentMethods paymentMethod);


        Task<Job> GetJobByIdWithBillingAsync(int jobId);


        Task UpdateJobOnlyAsync(Job job); // Example for specific update

        //a method to count jobs by statuses
        Task<int> CountJobsByStatusesAsync(IEnumerable<JobStatus> statuses);

        //a method to count jobs by status and billing information
        Task<int> CountJobsByStatusAndBillingAsync();

        // a method to count the jobs that are completed but not yet paid
        Task<int> CountCompletedJobsNotPaidAsync();

        // a method to get the recent jobs added to the system
        Task<List<Job>> GetRecentJobsAsync(int count);


        //a method to filter only recent jobs(e.g. last 50 days)
        Task<List<Job>> GetRecentJobsByDaysAsync(int days);


        //a method to get all jobs with billing paid or not paid
        Task<List<Job>> GetAllJobsWithBillingAsync(bool? isPaid = null);


        Task<JobDetailsViewModel> GetJobDetailsForViewByIdAsync(int jobId);


        // Method to get the monthly summary of jobs for a specific year
        Task<List<MonthlySummaryViewModel>> GetMonthlySummaryAsync(int year);


        //a method to get jobs needing attention, e.g., jobs that are overdue or have pending actions
        Task<List<JobAttentionViewModel>> GetJobsNeedingAttentionAsync(List<JobStatus> statuses,   int limit );


        //a method to calulate the total cost of a given job by its id
        Task<decimal> GetTotalComponentChargeForJobAsync(int jobId);


        //a method to return invoice view model for a given job id
        //Task<InvoiceViewModel> GetInvoiceDataAsync(int jobId);

        //search jobs by device name, customer name
        Task<List<Job>> SearchJobsAsync(string searchTerm);

        //a method to get all jobs with device and customer information
        Task<List<Job>> GetAllJobsWithDeviceAndCustomerInfoAsync();


        Task<bool> HasActiveUpgradeRecordsAsync(int jobId);






    }

}
