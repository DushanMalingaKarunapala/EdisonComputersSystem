using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        public int DeviceId { get; set; } // Foreign Key for Device
        public string? JobDescription { get; set; } // Description of the job

        public required DateTime CheckedInDate { get; set; }

        //renamed from TotalCost to ManualJobCost for clarity
        public decimal? ManualJobCost { get; set; } = 0.0m; // Total cost of the job

        public DateTime? CompletionDate { get; set; }

        public DateTime? CollectionDate { get; set; } // Date customer picked up the device


        //newly added peoperty : jobstatus
        public JobStatus JobStatus { get; set; } = JobStatus.CheckedIn; // Default job status


        //relationships

        public virtual Device? Device { get; set; } // Navigation Property to the Device being repaired
        public virtual Billing? Billing { get; set; }  // Billing information for the job

        public virtual ICollection<RepairRecord> RepairRecords { get; set; } = new List<RepairRecord>();
        public virtual ICollection<UpgradeRecord> UpgradeRecords { get; set; } = new List<UpgradeRecord>();
        public virtual ICollection<ServiceRecord> ServiceRecords { get; set; } = new List<ServiceRecord>();

    }
}
