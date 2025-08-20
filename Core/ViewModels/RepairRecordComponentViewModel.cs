using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class RepairRecordComponentViewModel
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; } // For display in EditRepairTask
        public int QuantityUsed { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
