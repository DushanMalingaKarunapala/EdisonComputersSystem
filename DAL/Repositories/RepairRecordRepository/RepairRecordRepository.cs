using Core.Enums;
using Core.IRepository;
using Core.IRepository.IRepairRecordRepository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.RepairRecordRepository
{
    public class RepairRecordRepository : Repository<RepairRecord>, IRepairRecordRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RepairRecordRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRepairRecordAsync(RepairRecord repairRecord)
        {
            //repair record jobid should already be set before calling this method

            //validate whether job exists
            var jobexists = _dbContext.Jobs.Any(j => j.JobId == repairRecord.JobId);
            if (!jobexists)
            {
                throw new KeyNotFoundException($"Job with ID {repairRecord.JobId} not found.");
            }

            await _dbContext.RepairRecords.AddAsync(repairRecord);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<RepairRecord> GetRepairRecordByIdWithComponentsAsync(int repairRecordId)
        {
            return await _dbContext.RepairRecords
                .Include(r => r.RepairRecordComponents)
                .ThenInclude(rrc => rrc.Component) // Assuming RepairRecordComponent has a navigation property to Component
                .FirstOrDefaultAsync(r => r.RepairRecordId == repairRecordId);

        }



        public async Task<RepairRecord> GetRepairRecordWithJobAsync(int repairRecordId)
        {
            return await _dbContext.RepairRecords
                .Include(r => r.Job) // Assuming RepairRecord has a navigation property to Job
                .FirstOrDefaultAsync(r => r.RepairRecordId == repairRecordId);
        }

        public async Task UpdateRepairRecordWithComponentAsync(RepairRecord repairRecord, List<RepairRecordComponent> componentstoAdd, List<int> componentIdstoRemove)
        {




            // Validate the repair record exists
            var existingRecord = await _dbContext.RepairRecords
                               .Include(r => r.RepairRecordComponents) // VERY IMPORTANT
                               .FirstOrDefaultAsync(r => r.RepairRecordId == repairRecord.RepairRecordId);

            if (existingRecord == null)
            {
                throw new KeyNotFoundException($"Repair record with ID {repairRecord.RepairRecordId} not found.");
            }
            // Update the existing repair record properties

            //_dbContext.Entry(existingRecord).CurrentValues.SetValues(repairRecord);

            existingRecord.ProblemDescription = repairRecord.ProblemDescription;
            existingRecord.RepairNotes = repairRecord.RepairNotes;
            existingRecord.RepairStatus = repairRecord.RepairStatus;
            existingRecord.WarrantyPeriodinMonths = repairRecord.WarrantyPeriodinMonths;


            //handle repair record components(smart update)

            var rrcsToRemove = existingRecord.RepairRecordComponents
        .Where(dbRrc => !componentstoAdd.Any(desiredRrc => desiredRrc.ComponentId == dbRrc.ComponentId))
        .ToList(); // Materialize the list before removing


            foreach (var rrcToRemove in rrcsToRemove)
            {
                _dbContext.RepairRecordComponents.Remove(rrcToRemove);
                // TODO: Consider stock adjustments (increase stock) if you manage stock
            }



            // Identify RRCs to add or update
            foreach (var desiredRrc in componentstoAdd)
            {
                var existingRrc = existingRecord.RepairRecordComponents
                    .FirstOrDefault(dbRrc => dbRrc.ComponentId == desiredRrc.ComponentId);

                if (existingRrc == null) // It's a new component for this repair
                {
                    // Ensure RepairRecordId is set for the new RRC
                    desiredRrc.RepairRecordId = existingRecord.RepairRecordId;
                    // ComponentId, QuantityUsed, PricePerUnit should be set on desiredRrc already
                    _dbContext.RepairRecordComponents.Add(desiredRrc); // Add the new join table entry
                                                                       // TODO: Consider stock adjustments (decrease stock)
                }
                else // It's an existing component link, check if quantity/price changed
                {
                    if (existingRrc.QuantityUsed != desiredRrc.QuantityUsed || existingRrc.PricePerUnit != desiredRrc.PricePerUnit)
                    {
                        // TODO: Consider stock adjustments based on quantity difference
                        existingRrc.QuantityUsed = desiredRrc.QuantityUsed;
                        existingRrc.PricePerUnit = desiredRrc.PricePerUnit;
                        _dbContext.Entry(existingRrc).State = EntityState.Modified;
                    }
                }
            }

            // Save all changes in one go
            await _dbContext.SaveChangesAsync();


        }


        public async Task<List<RepairRecord>> GetRepairRecordsByJobIdWithJobAsync(int jobId)
        {
            return await _dbContext.RepairRecords
            .Include(rt => rt.Job) // Eager load the parent Job
            .Where(rt => rt.JobId == jobId)
            .ToListAsync();


        }
    }
}
