using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UpgradeRecordComponent
    {
        public int UpgradeRecordId { get; set; } // Foreign Key to UpgradeRecord
        public virtual UpgradeRecord? UpgradeRecord { get; set; } // Navigation property
        public int ComponentId { get; set; } // Foreign Key to Component
        public virtual Component? Component { get; set; } // Navigation property
        //below properties were added newly to the UpgradeRecordComponent class
        public int QuantityUsed { get; set; } // Quantity of the component used in the upgrade
        public decimal PricePerUnit { get; set; } = 0.0m;

        // NEW: Specific warranty for this instance of the component in this upgrade
        public int? WarrantyPeriodInMonths { get; set; } // This is the value that matters for the invoice

        [NotMapped]
        public DateTime? WarrantyStartDate
        {
            get
            {
                // Get the parent Job through the UpgradeRecord navigation property
                var job = this.UpgradeRecord?.Job;

                // Ensure we have all necessary data
                if (job == null || !this.WarrantyPeriodInMonths.HasValue || this.WarrantyPeriodInMonths <= 0 || !job.CompletionDate.HasValue)
                {
                    return null;
                }

                // Define the end of the grace period
                DateTime gracePeriodEndDate = job.CompletionDate.Value.AddDays(7);

                // Check if the device has been collected
                if (job.JobStatus == Core.Enums.JobStatus.Collected && job.CollectionDate.HasValue)
                {
                    // If collected WITHIN the grace period, warranty starts on collection day.
                    if (job.CollectionDate.Value <= gracePeriodEndDate)
                    {
                        return job.CollectionDate.Value;
                    }
                    // If collected AFTER the grace period, warranty starts on the day the grace period ended.
                    else
                    {
                        return gracePeriodEndDate;
                    }
                }
                // If the device is ready but NOT yet collected, check if we are past the grace period.
                else if (
                    job.JobStatus == Core.Enums.JobStatus.TasksCompleted ||
                    job.JobStatus == Core.Enums.JobStatus.ReadyForBilling ||
                    job.JobStatus == Core.Enums.JobStatus.ReadyForPickup)
                {
                    if (DateTime.UtcNow > gracePeriodEndDate)
                    {
                        return gracePeriodEndDate;
                    }
                }

                // If none of the above conditions for starting the warranty have been met, it hasn't started yet.
                return null;
            }
        }

        [NotMapped]
        public DateTime? WarrantyExpiryDate
        {
            get
            {
                var startDate = this.WarrantyStartDate;
                if (startDate.HasValue && this.WarrantyPeriodInMonths.HasValue)
                {
                    return startDate.Value.AddMonths(this.WarrantyPeriodInMonths.Value);
                }
                return null;
            }
        }

    }
}
