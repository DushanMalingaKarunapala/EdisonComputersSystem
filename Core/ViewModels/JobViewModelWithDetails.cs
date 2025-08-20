using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class JobViewModelWithDetails
    {
        public int JobId { get; set; }
        public Device? Model { get; set; }
        public Customer? FullName { get; set; }
        public string? JobDescription { get; set; }
        public JobStatus JobStatus { get; set; }
        public string? Completed { get; set; }
        public decimal? ManualLaborCost { get; set; }
        public DateTime CheckedInDate { get; set; }

    }
}
