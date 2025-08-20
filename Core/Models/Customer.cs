using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string? LastName { get; set; }

        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        //readonly property for FullName
        public string FullName => $"{FirstName} {LastName}".Trim(); // this will return "FirstName LastName" or just "FirstName" if LastName is null

        [MaxLength(20)]

        //newly added property : recordstatus
        public RecordStatus RecordStatus { get; set; } = RecordStatus.Active;


        public required string Phone { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        public string? Address { get; set; } 

        // Relationships
        // A customer can have multiple devices
        public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
