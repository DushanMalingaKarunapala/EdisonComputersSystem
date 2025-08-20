using Core.Enums;
using Core.IRepository;
using Core.IRepository.IServiceRecordRepository;
using Core.Models;
using DAL.Repositories.ServiceRecordRepository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ServiceRecordService
{
    public class ServiceRecordService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceRecordRepository _serviceRecordRepository;


        public ServiceRecordService(IServiceProvider serviceProvider, IServiceRecordRepository serviceRecordRepository)
        {
            _serviceProvider = serviceProvider;
            _serviceRecordRepository = serviceRecordRepository;
        }

        public async Task AddServiceRecordAsync(Core.Models.ServiceRecord serviceRecord)
        {
            if (serviceRecord == null)
            {
                throw new ArgumentNullException(nameof(serviceRecord), "Service record cannot be null.");
            }
            // Validate that the job exists before adding the service record
            // Validate the job exists before adding the upgrade record
            if (serviceRecord.JobId <= 0)
            {
                throw new ArgumentException("Invalid Job ID for the upgrade record.");
            }


            await _serviceRecordRepository.AddServiceRecordAsync(serviceRecord);

            //update the job status from checkedin to in progress
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobService = scope.ServiceProvider.GetRequiredService<IRepository<Job>>();
                var job = await jobService.GetById(serviceRecord.JobId);
                if (job == null)
                {
                    throw new ArgumentException($"Job with ID {serviceRecord.JobId} does not exist.");
                }
                job.JobStatus = JobStatus.InProgress;
                await jobService.Update(job);
            }
        }

        public async Task<ServiceRecord> GetServiceRecordByIdAsync(int serviceRecordId)
        {
            if (serviceRecordId <= 0)
            {
                throw new ArgumentException("Invalid Service Record ID.");
            }
            return await _serviceRecordRepository.GetById(serviceRecordId);
        }



        //update service record method

        public async Task UpdateServiceRecordAsync(ServiceRecord serviceRecord)
        {
            if (serviceRecord == null)
            {
                throw new ArgumentNullException(nameof(serviceRecord), "Service record cannot be null.");
            }
            if (serviceRecord.ServiceRecordId <= 0)
            {
                throw new ArgumentException("Invalid Service Record ID.");
            }
            await _serviceRecordRepository.UpdateServiceRecord(serviceRecord);
        }


        public async Task<List<ServiceRecord>> GetServiceRecordsByJobIdAsync(int jobId)
        {
            if (jobId <= 0)
            {
                throw new ArgumentException("Invalid Job ID.");
            }
            return await _serviceRecordRepository.GetServiceRecordsByJobIdAsync(jobId);
        }
    }
}
