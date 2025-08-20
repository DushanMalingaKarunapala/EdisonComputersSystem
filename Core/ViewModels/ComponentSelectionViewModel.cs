using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class ComponentSelectionViewModel
    {
        public int ComponentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // Or ComponentType enum
        public decimal Price { get; set; } // Store's cost price
        public int QuantityInStock { get; set; }
        public bool IsSelected { get; set; } // For checkbox selection in grid
        public int QuantityToUse { get; set; } = 1; // Default to 1
        public decimal PricePerUnitOverride { get; set; } // If manual price entry is needed
        public int? WarrantyPeriodInMonths { get; set; } // Warranty period for this component in the upgrade
    }
}
