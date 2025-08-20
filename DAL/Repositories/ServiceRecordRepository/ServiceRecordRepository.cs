using Core.IRepository.IServiceRecordRepository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.ServiceRecordRepository
{

    public class ServiceRecordRepository : Repository<ServiceRecord>, IServiceRecordRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ServiceRecordRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddServiceRecordAsync(ServiceRecord serviceRecord)
        {
            //service record job id should already be set before calling this method
            //validate whether job exists
            var jobExists = _dbContext.Jobs.Any(j => j.JobId == serviceRecord.JobId);
            if (!jobExists)
            {
                throw new KeyNotFoundException($"Job with ID {serviceRecord.JobId} not found.");
            }
            await _dbContext.ServiceRecords.AddAsync(serviceRecord);
            await _dbContext.SaveChangesAsync();

        }

        //get service records by job id
        public async Task<List<ServiceRecord>> GetServiceRecordsByJobIdAsync(int jobId)
        {
            
            return await _dbContext.ServiceRecords
                .Include(sr => sr.Job) // Include the Job navigation property if needed
                .Where(sr => sr.JobId == jobId)
                .ToListAsync();

        }

        public Task UpdateServiceRecord(ServiceRecord serviceRecord)
        {
            if (serviceRecord == null)
            {
                throw new ArgumentNullException(nameof(serviceRecord), "Service record cannot be null.");
            }
            // Validate that the service record exists before updating
            var existingRecord = _dbContext.ServiceRecords.Find(serviceRecord.ServiceRecordId);
            if (existingRecord == null)
            {
                throw new KeyNotFoundException($"Service record with ID {serviceRecord.ServiceRecordId} not found.");
            }
            _dbContext.Entry(existingRecord).CurrentValues.SetValues(serviceRecord);
            return _dbContext.SaveChangesAsync();

        }

        
    }
}
