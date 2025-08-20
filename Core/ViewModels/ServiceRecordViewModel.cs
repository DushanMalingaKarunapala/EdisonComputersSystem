using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    //view model for service records.
    public class ServiceRecordViewModel
    {
        public int ServiceRecordId { get; set; }
  
        public string Description { get; set; }
        public string Note { get; set; }
        public string ServiceStatus { get; set; } // e.g., "Pending", "Completed", etc.
        public string ServiceType { get; set; } // Name of the technician who performed the service
    }
}
