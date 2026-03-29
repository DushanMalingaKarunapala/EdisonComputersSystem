using Core.Enums;
using Core.IRepository.IUpgradeRecordRepository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.UpgradeRecordRepository
{
    public class UpgradeRecordRepository : Repository<UpgradeRecord>, IUpgradeRecordRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UpgradeRecordRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUpgradeRecordAsync(UpgradeRecord upgraderRecord)
        {
            //upgrade reocrd job id should already be set before calling this method
            //validate whether job exists
            var jobExists = _dbContext.Jobs.Any(j => j.JobId == upgraderRecord.JobId);
            if (!jobExists)
            {
                throw new KeyNotFoundException($"Job with ID {upgraderRecord.JobId} not found.");
            }
            await _dbContext.UpgradeRecords.AddAsync(upgraderRecord);
            await _dbContext.SaveChangesAsync();


        }


        //method to get upgrade record by id with components included
        public async Task<UpgradeRecord> GetUpgradeRecordByIdWithComponentsAsync(int upgradeRecordId)
        {
            return await _dbContext.UpgradeRecords
             .Include(ur => ur.Job)
             .Include(u => u.UpgradeRecordComponents)
             .ThenInclude(urc => urc.Component) // Assuming upgradeRecordComponent has a navigation property to Component
             .FirstOrDefaultAsync(u => u.UpgradeRecordId == upgradeRecordId);

        }

        public async Task<List<UpgradeRecord>> GetUpgradeRecordsByJobIdAsync(int jobId)
        {
            return await _dbContext.UpgradeRecords
                .Include(ur=> ur.Job) // Include Job navigation property if needed
                .Where(ur => ur.JobId == jobId)
                .ToListAsync();



        }


        //method to update upgrade record with components (smart update)
        public async Task UpdateUpgradeRecordWithComponentAsync(UpgradeRecord upradeRecord, List<UpgradeRecordComponent> componentstoAdd, List<int> componentIdstoRemove)
        {

            // Validate the upgrade record exists
            var existingRecord = await _dbContext.UpgradeRecords
                               .Include(r => r.UpgradeRecordComponents) // VERY IMPORTANT
                               .FirstOrDefaultAsync(r => r.UpgradeRecordId == upradeRecord.UpgradeRecordId);

            if (existingRecord == null)
            {
                throw new KeyNotFoundException($"upgrade record with ID {upradeRecord.UpgradeRecordId} not found.");
            }
            // Update the existing upgrade record properties

            //_dbContext.Entry(existingRecord).CurrentValues.SetValues(upgradeRecord);

            existingRecord.UpgradeDescription = upradeRecord.UpgradeDescription;
            existingRecord.UpgradeStatus = upradeRecord.UpgradeStatus;
            //existingRecord.WarrantyExpiry = upradeRecord.WarrantyExpiry;
            existingRecord.UpgradeType = upradeRecord.UpgradeType;
            existingRecord.ServiceCharge = upradeRecord.ServiceCharge;
            


            //handle upgrade record components(smart update)

            var urcsToRemove = existingRecord.UpgradeRecordComponents
        .Where(dbUrc => !componentstoAdd.Any(desiredUrc => desiredUrc.ComponentId == dbUrc.ComponentId))
        .ToList(); // Materialize the list before removing


            foreach (var urcToRemove in urcsToRemove)
            {
                _dbContext.UpgradeRecordComponents.Remove(urcToRemove);
                // TODO: Consider stock adjustments (increase stock) if you manage stock
            }



            // Identify RRCs to add or update
            foreach (var desiredUrc in componentstoAdd)
            {
                var existingUrc = existingRecord.UpgradeRecordComponents
                    .FirstOrDefault(dbUrc => dbUrc.ComponentId == desiredUrc.ComponentId);

                if (existingUrc == null) // It's a new component for this upgrade
                {
                    // Ensure upgradeRecordId is set for the new RRC
                    desiredUrc.UpgradeRecordId = existingRecord.UpgradeRecordId;
                    // ComponentId, QuantityUsed, PricePerUnit should be set on desiredUrc already
                    _dbContext.UpgradeRecordComponents.Add(desiredUrc); // Add the new join table entry
                                                                       // TODO: Consider stock adjustments (decrease stock)
                }
                else // It's an existing component link, check if quantity/price changed
                {
                    if (existingUrc.QuantityUsed != desiredUrc.QuantityUsed || existingUrc.PricePerUnit != desiredUrc.PricePerUnit || existingUrc.WarrantyPeriodInMonths != desiredUrc.WarrantyPeriodInMonths)
                    {
                        // TODO: Consider stock adjustments based on quantity difference
                        existingUrc.QuantityUsed = desiredUrc.QuantityUsed;
                        existingUrc.PricePerUnit = desiredUrc.PricePerUnit;
                        existingUrc.WarrantyPeriodInMonths = desiredUrc.WarrantyPeriodInMonths;

                        _dbContext.Entry(existingUrc).State = EntityState.Modified;
                    }
                }
            }

            // Save all changes in one go
            await _dbContext.SaveChangesAsync();


        }
    }
}
