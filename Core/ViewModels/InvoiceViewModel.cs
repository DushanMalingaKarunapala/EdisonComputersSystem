using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    /// <summary>
    /// a view model for the invoice section in the application.
    public class InvoiceViewModel
    {
       
            // Shop Info (can be static or from config)
            public string ShopName { get; set; } = "Edison Computers";
            public string ShopAddress { get; set; } = "Kavindu Niwasa Wanduragala Kurunegala";
            public string ShopContact { get; set; } = "support@edisoncomputers.com | (555) 123-TECH";
            public byte[]? ShopLogoBytes { get; set; } // The logo data

            // Invoice & Job Info
            public int JobId { get; set; }
            public string? InvoiceNumber { get; set; }
            public DateTime InvoiceDate { get; set; } = DateTime.Now;

            // Customer & Device Info
            public string? CustomerFullName { get; set; }
            public string? CustomerAddress { get; set; }
            public string? CustomerPhone { get; set; }
            public string? CustomerEmail { get; set; }
            public string? DeviceIdentifier { get; set; }

            // Line Items for the table
            public List<InvoiceLineItem> LineItems { get; set; } = new List<InvoiceLineItem>();

            // Summary
            public decimal Subtotal { get; set; }
            public decimal ManualJobCost { get; set; }
            public decimal GrandTotal { get; set; }

            // Payment Info
            public bool IsPaid { get; set; }
            public string? PaymentStatusText { get; set; }
            public string? PaymentDetailsText { get; set; }

        //Billing Information
        public int billingId { get; set; }


    }

    public class InvoiceLineItem
        {
            public string? Description { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal LineTotal { get; set; }
            public string? WarrantyText { get; set; }
        }
    
}
