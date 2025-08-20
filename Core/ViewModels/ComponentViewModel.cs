using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class ComponentViewModel
    {
        //add properties for the component view model
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string Description { get; set; }
        public ComponentType ComponentType { get; set; }
        public string SerialNumber { get; set; }
        public decimal UnitPrice { get; set; }
        public int QuantityInStock { get; set; }
        public int? DefaultWarrantyInMonths { get; set; } // e.g., GPU = 24, RAM = 36, Used RAM = 0
    }
}
