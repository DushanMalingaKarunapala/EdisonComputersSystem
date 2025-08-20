using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class JobViewModel
    {
        public int JobId { get; set; }
        public string Description { get; set; }
        public string CustomerName { get; set; }
        public string Device { get; set; }
        public string JobStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public Decimal ManualLaborCost { get; set; }
        // Add other properties as needed
    }
}
