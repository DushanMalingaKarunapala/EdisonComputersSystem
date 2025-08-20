using Core.Enums;
using Core.IRepository.IComponentRepository;
using Core.IRepository.IJobRepository;
using Core.IRepository.IUpgradeRecordRepository;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.UpgradeRecordService
{
    public class UpgradeRecordService
    {
        private readonly IUpgradeRecordRepository _upgradeRecordRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IjobRepository _jobRepository;

        public UpgradeRecordService(IUpgradeRecordRepository upgradeRecordRepository, IServiceProvider serviceProvider ,IjobRepository jobRepository)
        {
            _upgradeRecordRepository = upgradeRecordRepository;
            _serviceProvider = serviceProvider;
            _jobRepository = jobRepository;
        }

        public async Task AddUpgradeRecordAsync(UpgradeRecord upgradeRecord)
        {
            // Validate the job exists before adding the upgrade record
            if (upgradeRecord.JobId <= 0)
            {
                throw new ArgumentException("Invalid Job ID for the upgrade record.");
            }
            
            await _upgradeRecordRepository.AddUpgradeRecordAsync(upgradeRecord);

            //update the job status to 


        }

        public async Task<UpgradeRecord> GetUpgradeRecordbyIdwithComponents(int upgradeRecordId)
        {
            if (upgradeRecordId <= 0)
            {
                throw new ArgumentException("Invalid Upgrade Record ID.");
            }
            return await _upgradeRecordRepository.GetUpgradeRecordByIdWithComponentsAsync(upgradeRecordId);
        }

        public async Task UpdateUpgradeRecordWithComponentAsync(UpgradeRecord upgradeRecord, List<UpgradeRecordComponent> componentsToAdd, List<int> componentIdsToRemove)
        {
            //validate the upgrade record exists
            // 1. Get the current state of the UpgradeRecord WITH its components from the DB
            //    This is crucial for knowing what was there before.
            var existingRecord = await _upgradeRecordRepository.GetUpgradeRecordByIdWithComponentsAsync(upgradeRecord.UpgradeRecordId);
            if (existingRecord == null)
            {
                throw new KeyNotFoundException($"Upgrade record with ID {upgradeRecord.UpgradeRecordId} not found.");
            }

            // Any other business logic, stock adjustments etc. can go here
            // For stock, you'd need to compare 'desiredFinalComponents' with 'existingRecord.upgradeRecordComponents' *before* calling the repo.

            // 2. Calculate stock changes BEFORE applying updates to the database
            //    This requires comparing existingupgradeRecord.upgradeRecordComponents with componentstoad.

            var stockAdjustments = new Dictionary<int, int>(); // ComponentId -> QuantityChange (positive to add back, negative to decrease)


            // Components that were in the DB but are NOT in the desired list (they are being removed)
            foreach (var dbUrc in existingRecord.UpgradeRecordComponents)
            {
                if (!componentsToAdd.Any(desired => desired.ComponentId == dbUrc.ComponentId))
                {
                    // This component link is being removed entirely, add its quantity back to stock
                    if (!stockAdjustments.ContainsKey(dbUrc.ComponentId)) stockAdjustments[dbUrc.ComponentId] = 0;
                    stockAdjustments[dbUrc.ComponentId] += dbUrc.QuantityUsed; // Positive: add back
                }
            }

            // Components that are in the desired list (either new or potentially updated quantity)
            foreach (var desiredUrc in componentsToAdd)
            {
                var dbUrcEquivalent = existingRecord.UpgradeRecordComponents
                    .FirstOrDefault(dbUrc => dbUrc.ComponentId == desiredUrc.ComponentId);

                if (dbUrcEquivalent == null) // This is a NEW component being added to the upgrade
                {
                    if (!stockAdjustments.ContainsKey(desiredUrc.ComponentId)) stockAdjustments[desiredUrc.ComponentId] = 0;
                    stockAdjustments[desiredUrc.ComponentId] -= desiredUrc.QuantityUsed; // Negative: decrease stock
                }
                else // This component link already existed; check for quantity change
                {
                    int quantityDifference = desiredUrc.QuantityUsed - dbUrcEquivalent.QuantityUsed;
                    if (quantityDifference != 0)
                    {
                        if (!stockAdjustments.ContainsKey(desiredUrc.ComponentId)) stockAdjustments[desiredUrc.ComponentId] = 0;
                        stockAdjustments[desiredUrc.ComponentId] -= quantityDifference; // Negative if new qty > old qty (decrease stock)
                                                                                        // Positive if new qty < old qty (increase stock)
                    }
                }
            }

            // 3. Apply stock adjustments (BEFORE saving upgradeRecord changes to ensure stock availability)
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

            await _upgradeRecordRepository.UpdateUpgradeRecordWithComponentAsync(upgradeRecord, componentsToAdd, componentIdsToRemove);
        }

        //get upgrade record by job id
        public async Task<List<UpgradeRecord>> GetUpgradeRecordsByJobIdAsync(int jobId)
        {
            if (jobId <= 0)
            {
                throw new ArgumentException("Invalid Job ID.");
            }
            return await _upgradeRecordRepository.GetUpgradeRecordsByJobIdAsync(jobId);
        }


    }
}
