using Core.Common;
using Core.Enums;
using Core.IRepository;
using Core.IRepository.IComponentRepository;
using Core.IRepository.IJobRepository;
using Core.IRepository.IRepairRecordRepository;
using Core.Models;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.RepairRecordService
{
    
    public class RepairRecordService
    {
        private readonly IRepairRecordRepository _repairRecordRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IjobRepository _jobRepository;


        public RepairRecordService(IRepairRecordRepository repairRecordRepository, IServiceProvider serviceProvider, IjobRepository jobRepository)
        {
            _repairRecordRepository = repairRecordRepository;
            _serviceProvider = serviceProvider;
            _jobRepository = jobRepository;
        }

        public async Task AddRepairRecordAsync(RepairRecord repairRecord)
        {
            try
            {
                LoggingService.LogInformation($"Adding repair record for job {repairRecord.JobId}: {repairRecord.ProblemDescription}");

                


                await _repairRecordRepository.AddRepairRecordAsync(repairRecord);

                LoggingService.LogInformation($"Successfully added repair record: {repairRecord.RepairRecordId} for job {repairRecord.JobId}");

                //update the job status from checkedin to in progress
                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<IRepository<Job>>();
                    var job = await jobService.GetById(repairRecord.JobId);
                    if (job == null)
                    {
                        throw new ArgumentException($"Job with ID {repairRecord.JobId} does not exist.");
                    }
                    job.JobStatus = JobStatus.InProgress;
                    await jobService.Update(job);
                }

            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to add repair record for job {repairRecord.JobId}");
                throw;
            }


        }

        public async Task<RepairRecord> GetRepairRecordbyIdwithComponents(int repairRecordId)
        {
            //get the repair record by id with components
            return await _repairRecordRepository.GetRepairRecordByIdWithComponentsAsync(repairRecordId);
        }


        public async Task UpdateRepairRecordWithComponentAsync(RepairRecord repairRecord, List<RepairRecordComponent> componentstoAdd, List<int> componentIdstoRemove)
        {

            try
            {
                LoggingService.LogInformation($"Updating repair record {repairRecord.RepairRecordId} for job {repairRecord.JobId}");
                //validate the repair record exists
                // 1. Get the current state of the RepairRecord WITH its components from the DB
                //    This is crucial for knowing what was there before.
                var existingRecord = await _repairRecordRepository.GetRepairRecordByIdWithComponentsAsync(repairRecord.RepairRecordId);
                if (existingRecord == null)
                {
                    throw new KeyNotFoundException($"Repair record with ID {repairRecord.RepairRecordId} not found.");
                }

                // Any other business logic, stock adjustments etc. can go here
                // For stock, you'd need to compare 'desiredFinalComponents' with 'existingRecord.RepairRecordComponents' *before* calling the repo.

                // 2. Calculate stock changes BEFORE applying updates to the database
                //    This requires comparing existingRepairRecord.RepairRecordComponents with componentstoad.

                var stockAdjustments = new Dictionary<int, int>(); // ComponentId -> QuantityChange (positive to add back, negative to decrease)


                // Components that were in the DB but are NOT in the desired list (they are being removed)
                foreach (var dbRrc in existingRecord.RepairRecordComponents)
                {
                    if (!componentstoAdd.Any(desired => desired.ComponentId == dbRrc.ComponentId))
                    {
                        // This component link is being removed entirely, add its quantity back to stock
                        if (!stockAdjustments.ContainsKey(dbRrc.ComponentId)) stockAdjustments[dbRrc.ComponentId] = 0;
                        stockAdjustments[dbRrc.ComponentId] += dbRrc.QuantityUsed; // Positive: add back
                    }
                }

                // Components that are in the desired list (either new or potentially updated quantity)
                foreach (var desiredRrc in componentstoAdd)
                {
                    var dbRrcEquivalent = existingRecord.RepairRecordComponents
                        .FirstOrDefault(dbRrc => dbRrc.ComponentId == desiredRrc.ComponentId);

                    if (dbRrcEquivalent == null) // This is a NEW component being added to the repair
                    {
                        if (!stockAdjustments.ContainsKey(desiredRrc.ComponentId)) stockAdjustments[desiredRrc.ComponentId] = 0;
                        stockAdjustments[desiredRrc.ComponentId] -= desiredRrc.QuantityUsed; // Negative: decrease stock
                    }
                    else // This component link already existed; check for quantity change
                    {
                        int quantityDifference = desiredRrc.QuantityUsed - dbRrcEquivalent.QuantityUsed;
                        if (quantityDifference != 0)
                        {
                            if (!stockAdjustments.ContainsKey(desiredRrc.ComponentId)) stockAdjustments[desiredRrc.ComponentId] = 0;
                            stockAdjustments[desiredRrc.ComponentId] -= quantityDifference; // Negative if new qty > old qty (decrease stock)
                                                                                            // Positive if new qty < old qty (increase stock)
                        }
                    }
                }

                // 3. Apply stock adjustments (BEFORE saving RepairRecord changes to ensure stock availability)
                //    It's important to do this carefully, potentially within the same transaction.
                //    The repository method for updating stock should handle concurrency.
                using (var scope = _serviceProvider.CreateScope()) // Assuming _serviceProvider is available
                {
                    var componentRepository = scope.ServiceProvider.GetRequiredService<IComponentRepository>();
                    foreach (var adjustment in stockAdjustments)
                    {
                        int componentId = adjustment.Key;
                        int quantityChange = adjustment.Value;

                        if (quantityChange > 0) // Add back to stock
                        {
                            await componentRepository.IncreaseStockAsync(componentId, quantityChange);
                        }
                        else if (quantityChange < 0) // Decrease from stock
                        {
                            // Check for sufficient stock BEFORE decreasing
                            var component = await componentRepository.GetById(componentId); // Fetch current component
                            if (component == null || component.QuantityInStock < -quantityChange) // -quantityChange because it's negative
                            {
                                throw new InvalidOperationException($"Insufficient stock for component ID {componentId}. Required: {-quantityChange}, Available: {component?.QuantityInStock ?? 0}.");
                            }
                            await componentRepository.DecreseStockAsync(componentId, -quantityChange);
                        }
                    }
                }



                await _repairRecordRepository.UpdateRepairRecordWithComponentAsync(repairRecord, componentstoAdd, componentIdstoRemove);
                LoggingService.LogInformation($"Successfully updated repair record: {repairRecord.RepairRecordId} for job {repairRecord.JobId}");

            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to update repair record for job {repairRecord.JobId}");
                throw;
            }
        }

        public async Task<List<RepairRecord>> GetRepairRecordsByJobIdAsync(int jobId)
        {
            // Simple pass-through to the new repository method
            return await _repairRecordRepository.GetRepairRecordsByJobIdWithJobAsync(jobId);
        }



    }
    
}
