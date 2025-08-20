using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UpgradeRecord
    {
        [Key]
        public int UpgradeRecordId { get; set; }
        public int JobId { get; set; } // Foreign Key to Job
        public UpgradeType UpgradeType { get; set; } // Enum for upgrade type
        //newly added properties

        //this property is removed as new property called defaultwarrantyperiodinmonths is added
        //public DateTime WarrantyExpiry { get; set; } // Warranty expiry date for the upgrade



        public string? UpgradeDescription { get; set; } // Optional description of the upgrade
        public UpgradeStatus UpgradeStatus { get; set; } = UpgradeStatus.Requested; // Default status
        //removed Cost as it was not needed in the context of upgrade records

        // Navigation property
        //public virtual Device? Device { get; set; }

        public virtual Job? Job { get; set; } // Navigation property to Job


        public virtual ICollection<UpgradeRecordComponent>? UpgradeRecordComponents { get; set; } = new List<UpgradeRecordComponent>();
    }
}
