using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class MonthlySummaryViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } // e.g., "Jan", "Feb"
        public decimal TotalSales { get; set; }
        public decimal TotalProfit { get; set; }
    }
}
