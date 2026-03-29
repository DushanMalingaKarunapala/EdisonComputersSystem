using Core.Common;
using Core.Enums;
using Core.IRepository;
using Core.IRepository.IJobRepository;
using Core.Models;
using Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.JobService
{
    public class JobService
    {
        private readonly IjobRepository _jobRepository;
        private readonly IRepository<Job> _genericRepository;
        private readonly IServiceProvider _serviceProvider;



        public JobService(IjobRepository jobRepository, IRepository<Job> genericRepository, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _jobRepository = jobRepository;
            _genericRepository = genericRepository;
        }
        public async Task<IEnumerable<Job>> GetAllJobs()
        {
            return (await _genericRepository.GetAll()).ToList();
        }

        public async Task<Job> GetJobById(int id)
        {
            return await _genericRepository.GetById(id);
        }

        public async Task<Job> AddJob(int customerId, int deviceId, Job job)
        {
            try
            {
                LoggingService.LogInformation($"Creating new job for device {deviceId}, customer {customerId}");

                //validate whether the customer with this id exists or not
                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<IRepository<Customer>>();
                    var customer = await customerService.GetById(customerId);
                    if (customer == null)
                    {
                        throw new ArgumentException($"Customer with ID {customerId} does not exist.");
                    }
                }

                //validate that the device with this id exists and that it belongs to the given customerid 
                //this is importnt to ensure that the job is being added to a device that belongs to the customer
                using (var scope = _serviceProvider.CreateScope())
                {
                    var deviceService = scope.ServiceProvider.GetRequiredService<IRepository<Device>>();
                    var device = await deviceService.GetById(deviceId);
                    if (device == null || device.CustomerId != customerId)
                    {
                        throw new ArgumentException($"Device with ID {deviceId} does not exist or does not belong to Customer with ID {customerId}.");
                    }
                }


                // If both validations pass, proceed to add the job

                job.DeviceId = deviceId; // Set the device ID for the job


                //call the repository to add the job
                // Note: The job.DeviceId is already set above, so we don't need to pass it again
                var result = await _jobRepository.AddJob(deviceId, job);

                return result;


            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to create job for device {deviceId}, customer {customerId}");
                throw;
            }


        }





        public async Task<Job> UpdateJob(int jobId, Job jobToUpdate)
        {
            try
            {
                LoggingService.LogInformation($"Updating job with ID {jobId}");

                return await _jobRepository.UpdateJob(jobId, jobToUpdate);

            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to update job with ID {jobId}");
                throw;
            }
        }

        public async Task<Job> GetJob(int jobId)
        {
            return await _jobRepository.GetJobByIdWithAllDetailsForBillingAsync(jobId);
        }


        //method to save or update job billing information
        public async Task SaveOrUpdateJobBillingAsync(
        int jobId, decimal finalBillingAmount, decimal manualJobCost,
        bool isPaid, DateTime? paymentDate, Core.Enums.PaymentMethods paymentMethod)
        {
            try
            {
                LoggingService.LogInformation($"Saving billing for job {jobId}: Amount={finalBillingAmount:C}, Paid={isPaid}");

                // Resolve repositories within the service method if not injected class-wide (using IServiceProvider for per-call scope)
                // Or use injected repositories if service is scoped per operation.

                var job = await _jobRepository.GetJobByIdWithBillingAsync(jobId); // Method to get job and its billing
                if (job == null) throw new KeyNotFoundException($"Job with ID {jobId} not found.");


                // Update Job's ManualJobCost
                job.ManualJobCost = manualJobCost;
                // No need to update job.Billing.Amount here if billingAmount is from the textbox,
                // that will be set on the Billing entity directly.

                if (job.Billing == null)
                {
                    // Create new Billing record
                    job.Billing = new Billing
                    {
                        JobId = jobId, // Already set by relationship if you add to job.Billing and save job
                        Amount = finalBillingAmount,
                        IsPaid = isPaid,
                        PaymentDate = paymentDate,
                        PaymentMethod = paymentMethod
                    };
                    // EF Core will handle adding this new Billing entity when job is saved,
                    // or add it explicitly to _dbContext.Billings if job.Billing = newBilling; isn't enough.
                    // await _billingRepository.AddAsync(job.Billing); // if you have a billing repo
                }
                else
                {
                    // Update existing Billing record
                    job.Billing.Amount = finalBillingAmount;
                    job.Billing.IsPaid = isPaid;
                    job.Billing.PaymentDate = paymentDate;
                    job.Billing.PaymentMethod = paymentMethod;
                    // _billingRepository.UpdateAsync(job.Billing); // if you have a billing repo
                }

                // The JobRepository's UpdateAsync should handle saving the Job and its related Billing changes
                await _jobRepository.UpdateJobOnlyAsync(job); // A method that only saves Job and its direct relations like Billing
                                                              // Or if your generic Update handles related entities well.
                                                              // Alternatively, just context.SaveChangesAsync() if all changes are tracked.
                LoggingService.LogInformation($"Successfully saved billing for job {jobId}");

            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to save billing for job {jobId}");
                throw;
            }
        }


        //dedicated update jobstatus method to update the job status

        public async Task UpdateJobStatusAsync(int jobId, JobStatus newStatus)
        {
            try
            {
                LoggingService.LogInformation($"Updating job {jobId} status to {newStatus}");
                // 1. Fetch the existing job from the database
                var job = await _jobRepository.GetById(jobId); // Assuming generic GetByIdAsync
                if (job == null)
                {
                    throw new KeyNotFoundException($"Job with ID {jobId} not found.");
                }

                // Store the old status for comparison if needed
                var oldStatus = job.JobStatus;

                // --- Apply Business Rules ---
                job.JobStatus = newStatus; // Update to the new status

                // Rule: If status moves to TasksCompleted, ReadyForBilling, or ReadyForPickup for the first time,
                // and a CompletionDate isn't already set, set it now. This marks when the *work* was finished.
                if ((newStatus == JobStatus.TasksCompleted || newStatus == JobStatus.ReadyForBilling || newStatus == JobStatus.ReadyForPickup)
                    && !job.CompletionDate.HasValue)
                {
                    job.CompletionDate = DateTime.Now;
                }

                // Rule: If status moves to Collected, set the CollectionDate.
                // Also, ensure CompletionDate is set if for some reason it wasn't already.
                if (newStatus == JobStatus.Collected)
                {
                    if (!job.CollectionDate.HasValue) // Only set it once
                    {
                        job.CollectionDate = DateTime.Now;
                    }

                    if (!job.CompletionDate.HasValue) // Safety net
                    {
                        job.CompletionDate = job.CollectionDate;
                    }
                }

                // 2. Call the repository to save the updated job
                // It's better to call the generic Update, as it will save all modified properties.
                await _jobRepository.Update(job);

                LoggingService.LogInformation($"Successfully updated job {jobId} status to {newStatus}");

            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to update job {jobId} status to {newStatus}");
                throw;
            }
        }


        public async Task<List<Job>> GetJobsByStatusAsync(JobStatus status)
        {
            return await _jobRepository.GetJobsByStatusAsync(status);
        }

        public async Task<IEnumerable<Job>> GetJobsByDeviceIdAsync(int deviceId)
        {
            return await _jobRepository.GetJobsByDeviceIdAsync(deviceId);
        }

        //get recent jobs added to the system
        public async Task<List<Job>> GetRecentJobsAsync(int days)
        {
            return await _jobRepository.GetRecentJobsByDaysAsync(days);
        }

        //get all jobs with billing information with paid or not paid
        public async Task<List<Job>> GetAllJobsWithBillingAsync(bool? isPaid = null)
        {
            return await _jobRepository.GetAllJobsWithBillingAsync(isPaid);
        }



        public async Task<JobDetailsViewModel> GetJobDetailsForViewByIdAsync(int jobId)
        {
            var jobDetails = await _jobRepository.GetJobDetailsForViewByIdAsync(jobId);
            if (jobDetails == null)
            {
                throw new KeyNotFoundException($"Job with ID {jobId} not found.");
            }
            // You can add more business logic here if needed
            return jobDetails;
        }


        //cancel a job by setting its status to canceled
        public async Task CancelJobAsync(int jobId)
        {
            // Fetch the job
            var job = await _jobRepository.GetById(jobId);
            if (job == null)
            {
                throw new KeyNotFoundException($"Job with ID {jobId} not found.");
            }
            // Set the status to Canceled
            job.JobStatus = JobStatus.Cancelled;
            // Optionally, you can set the CompletionDate or CollectionDate to null if needed
            job.CompletionDate = null;
            job.CollectionDate = null;
            // Save the changes
            await _jobRepository.Update(job);


        }


        //method to calculate the total cost of a job
        public async Task<decimal> GetTotalComponentChargeForJobAsync(int jobId)
        {
            return await _jobRepository.GetTotalComponentChargeForJobAsync(jobId);
        }


        //method to search jobs by device name and customer name
        public async Task<List<Job>> SearchJobsByDeviceAndCustomerAsync(string searchTerm)
        {


            // This method should implement the logic to search jobs by device name and customer name
            // For example, you can use a repository method that filters jobs based on the search term
            return await _jobRepository.SearchJobsAsync(searchTerm);
        }


        //a method to get all jobs with device and customer information
        public async Task<List<Job>> GetAllJobsWithDeviceAndCustomerAsync()
        {
            // This method should implement the logic to get all jobs with device and customer information
            // For example, you can use a repository method that includes related entities
            return await _jobRepository.GetAllJobsWithDeviceAndCustomerInfoAsync();
        }


        //a method to check if the job has existing upgrade records
        public async Task<bool> HasExistingUpgradeRecordsAsync(int jobId)
        {
            //check job id is valid
            if (jobId <= 0)
            {
                throw new ArgumentException("Invalid job ID.", nameof(jobId));
            }

            return await _jobRepository.HasActiveUpgradeRecordsAsync(jobId);
        }


        //new method to sum up all service charges for a given job by its id -- newly added on 2026-03-24
        public async Task<decimal> GetTotalServiceChargeForJobAsync(int jobId)
        {
            //check job id is valid
            if (jobId <= 0)
            {
                throw new ArgumentException("Invalid job ID.", nameof(jobId));
            }
            return await _jobRepository.GetTotalServiceChargeForJobAsync(jobId);
        }
    }
}
