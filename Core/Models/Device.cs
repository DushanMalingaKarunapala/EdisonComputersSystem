using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; }
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Brand { get; set; }

        [MaxLength(150)]
        public string? Model { get; set; }

        [MaxLength(100)]
        public string? SerialNumber { get; set; } 

        public DeviceType DeviceType { get; set; } // Enum for device type

        // This  represents the first time this specific device (by serial number)
        // was registered in the system, not necessarily the check-in date for a repair.
        public DateTime ReceivedDate { get; set; }


        //newly added property : recordstatus
        public RecordStatus RecordStatus { get; set; } = RecordStatus.Active;

        // --- Relationships ---

        // Foreign Key for Customer
        public int CustomerId { get; set; }
        // Navigation Property to the Owner Customer
        public virtual Customer? Customer { get; set; }


        // A device can have multiple images
        public virtual ICollection<DeviceImage> DeviceImages { get; set; } = new List<DeviceImage>();

        //a device can have multiple jobs
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();   
    }
}
