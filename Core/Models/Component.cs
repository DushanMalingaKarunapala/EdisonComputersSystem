using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Component
    {
        [Key]
        public int ComponentId { get; set; }
        public string? SerialNumber { get; set; } // Unique serial number for the component
        public string? Name { get; set; }
        public ComponentType Type { get; set; } // Type of the component (e.g., GPU, RAM, etc.)
        public string? Description { get; set; }
        public required decimal Price { get; set; } // Price of the component
        public int QuantityInStock { get; set; } // Quantity available in stock
        //newly added property - to represent the stock status and availability of the component
        public ComponentStockStatus.StockStatus StockStatus { get; set; } = ComponentStockStatus.StockStatus.InStock;  // Stock status of the component
        //default warranty for the component
        public int? DefaultWarrantyInMonths { get; set; } // e.g., GPU = 24, RAM = 36, Used RAM = 0

        // Relationships

        // A component can be part of multiple repair records

        public virtual ICollection<RepairRecordComponent> RepairRecordComponents { get; set; } = new List<RepairRecordComponent>();

        public virtual ICollection<UpgradeRecordComponent> UpgradeRecordComponents { get; set; } = new List<UpgradeRecordComponent>();
    }
}
