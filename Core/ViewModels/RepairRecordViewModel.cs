using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class RepairRecordViewModel // a view model for the repair records
    {
        public int RepairRecordId { get; set; }
        public string Description { get; set; }
        public string RepairStatus { get; set; } // e.g., "Pending", "In Progress", "Completed", "Cancelled"
        public string Notes { get; set; }
        public string WarrantyExpiryDate { get; set; } // Assuming this is a string for display purposes, could be DateTime if needed


    }
}
