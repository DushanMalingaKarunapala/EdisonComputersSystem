using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class InvoiceItemViewModel
    {
        public string TaskDescription { get; set; }
        public string ComponentName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string WarrantyInfo { get; set; } // e.g., "Warranty: 06/30/2025"
    }
}
