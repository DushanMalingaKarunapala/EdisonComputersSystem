using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class JobDetailsViewModel
    {
        // Job Info
        public int JobId { get; set; }
        public string JobDescription { get; set; }
        public JobStatus JobStatus { get; set; }
        public DateTime CheckedInDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal ManualJobCost { get; set; }

        // Device Info
        public string DeviceModel { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceSerialNumber { get; set; }

        // Customer Info
        public string CustomerFullName { get; set; }
        public string DeviceIdentifier { get; set; } // e.g., "Dell Inspiron (SN: ABC)"


        // Billing Info
        public int? BillingId { get; set; }
        public decimal BillingAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public bool BillingExists { get; set; } // Flag to know if a bill was found


        // Calculated Values
        public decimal TotalComponentChargeToCustomer { get; set; }
        public decimal ActualStoreCostForComponents { get; set; }

        public decimal TotalServiceCharge { get; set; } //newly added on 2026.03.24 to capture total service charge from all tasks

        public decimal CalculatedProfit { get; set; }
    }
}
