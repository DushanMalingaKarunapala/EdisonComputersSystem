using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class UpgradeRecordComponentViewModel
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; } // For display in EditRepairTask
        public int QuantityUsed { get; set; }
        public decimal PricePerUnit { get; set; }
        public int? WarrantyPeriodInMonths { get; set; } // This is the value that matters for the invoice
        public string? WarrantyExpiryDate { get; set; } // Assuming this is a string for display purposes, could be DateTime if needed

    }
}
