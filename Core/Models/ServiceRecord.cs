using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public ServiceType ServiceType { get; set; } = ServiceType.GenaralService; // Default service type
        public ServiceStatus ServiceStatus { get; set; } = ServiceStatus.Scheduled; // Default service status
        //removed Cost as it was not needed in the context of service records
        public virtual Job? Job { get; set; } // Navigation property to Job

    }
}
