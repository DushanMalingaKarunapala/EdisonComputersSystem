using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ServiceRecord
    {
        [Key]
        public int ServiceRecordId { get; set; }
        public int JobId { get; set; } // Foreign Key for Job

        public string? ServiceDescription { get; set; }
        public string? Note { get; set; }

        //new property to hold the service charge separately from the component costs, as not all repairs will have components
        // Add new property for service charge
        [Column(TypeName = "decimal(18,2)")]
        public decimal ServiceCharge { get; set; } = 0; // Default to 0

        public ServiceType ServiceType { get; set; } = ServiceType.GenaralService; // Default service type
        public ServiceStatus ServiceStatus { get; set; } = ServiceStatus.Scheduled; // Default service status
        //removed Cost as it was not needed in the context of service records
        public virtual Job? Job { get; set; } // Navigation property to Job

    }
}
