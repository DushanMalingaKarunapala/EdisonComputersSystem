using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class JobAttentionViewModel
    {
        public int JobId { get; set; }
        public string CustomerName { get; set; }
        public string DeviceIdentifier { get; set; } // e.g., "Dell Inspiron 15"
        public string Status { get; set; }
        public int DaysOpen { get; set; }
    }
}
