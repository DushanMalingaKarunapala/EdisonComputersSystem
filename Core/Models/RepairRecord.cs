using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// In RepairRecord.cs
/// <summary>
/// The numeric duration of the warranty. Despite the name, this can be in Days or Months,
/// as determined by the WarrantyUnit property.
/// </summary>
namespace Core.Models
{
    public class RepairRecord
    {
        [Key]
        public int RepairRecordId { get; set; }

        public int JobId { get; set; } // Foreign Key for Job

        public string? ProblemDescription { get; set; }

        // Renamed from RepairDetails for clarity - what the technician did
        public string? RepairNotes { get; set; } //work log

        // NEW: Status tracking based on  dropdown/list
        public RepairStatus RepairStatus { get; set; } = RepairStatus.Pending; // Default status

        // Made nullable as not all repairs might have/need a warranty
        //removed this warrantyexpiry property added warrantyperiodinmonths property instead
        //public DateTime? WarrantyExpiry { get; set; }

        public int? WarrantyPeriodinMonths { get; set; } = 0; // Default to 0 months // even tho this says months, it can be days or months based on the WarrantyUnit

        // New property to determine if the warranty is in days or months
        public WarrantyUnit WarrantyUnit { get; set; } = WarrantyUnit.Months; // Default to Months

        public virtual Job? Job { get; set; } // Navigation Property to the Job being repaired


        public virtual ICollection<RepairRecordComponent>? RepairRecordComponents { get; set; } = new List<RepairRecordComponent>();



        //create unmapped warrantystartdate and warrantyenddate properties for easy access
        [NotMapped]
        public DateTime? WarrantyStartDate
        {
            get
            {
                // 1. Initial checks: No job, no warranty period, or no completion date means no warranty can start.
                if (this.Job == null || !this.WarrantyPeriodinMonths.HasValue || this.WarrantyPeriodinMonths <= 0 || !this.Job.CompletionDate.HasValue)
                {
                    return null;
                }

                // 2. Define the end of the grace period.
                DateTime gracePeriodEndDate = this.Job.CompletionDate.Value.AddDays(7);

                // 3. Check if the device has been collected.
                if (this.Job.JobStatus == JobStatus.Collected && this.Job.CollectionDate.HasValue)
                {
                    // If collected WITHIN the grace period, warranty starts on collection day.
                    if (this.Job.CollectionDate.Value <= gracePeriodEndDate)
                    {
                        return this.Job.CollectionDate.Value;
                    }
                    // If collected AFTER the grace period, warranty starts on the day the grace period ended.
                    else
                    {
                        return gracePeriodEndDate;
                    }
                }
                // 4. If the device is ready but NOT yet collected, check if we are past the grace period.
                else if (
                    this.Job.JobStatus == JobStatus.TasksCompleted ||
                    this.Job.JobStatus == JobStatus.ReadyForBilling ||
                    this.Job.JobStatus == JobStatus.ReadyForPickup)
                {
                    // If today is past the grace period end date, the warranty has started.
                    if (DateTime.UtcNow > gracePeriodEndDate)
                    {
                        return gracePeriodEndDate;
                    }
                }

                // If none of the above conditions for starting the warranty have been met, it hasn't started yet.
                return null;
            }
        }

        // The WarrantyExpiryDate property remains the same, as it correctly uses WarrantyStartDate.
        [NotMapped]
        public DateTime? WarrantyExpiryDate
        {
            get
            {
                var startDate = this.WarrantyStartDate;
                if (startDate.HasValue && this.WarrantyPeriodinMonths.HasValue && this.WarrantyPeriodinMonths>0)
                {
                    // Use the new WarrantyUnit to calculate the correct expiry date
                    if (this.WarrantyUnit == WarrantyUnit.Days)
                    {
                        return startDate.Value.AddDays(this.WarrantyPeriodinMonths.Value);
                    }
                    else // Default to Months
                    {
                        return startDate.Value.AddMonths(this.WarrantyPeriodinMonths.Value);
                    }


                }
                return null;
            }
        }

    }
}
