using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class RecentActivityViewModel
    {
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
        //public string IconClass { get; set; } // e.g., "fas fa-plus-circle"
        //public string IconColor { get; set; } // e.g., "#2ecc71"
    }
}
