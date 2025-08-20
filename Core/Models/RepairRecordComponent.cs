using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class RepairRecordComponent
    {
        // Foreign Key for Repair Record
        public int RepairRecordId { get; set; }
        public virtual RepairRecord? RepairRecord { get; set; }
        // Foreign Key for Component
        public int ComponentId { get; set; }
        public virtual Component? Component { get; set; }
        // Quantity of this component used in the repair
        public int QuantityUsed { get; set; } = 1;
        // Optional: Could add a price per unit if needed
        public decimal PricePerUnit { get; set; } = 0.0m;
    }
}
