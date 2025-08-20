
using Core.Enums;
using System.ComponentModel.DataAnnotations;


namespace Core.Models
{
    public class Billing
    {
        // Using RepairRecordId as the PK makes it a true 1-to-1 shared primary key relationship
        // public int RepairRecordId { get; set; }
        // OR keep a separate Id if you prefer independent keys (more common)
        [Key] // Primary Key
        public int BillingId { get; set; } 

        public int JobId { get; set; } // Foreign Key for Job
        [Required]
        [Range(0, 9999999.99)] // Example range validation
        public decimal Amount { get; set; }

        public bool? IsPaid { get; set; } = false;

        public DateTime? PaymentDate { get; set; } // Nullable, only set when paid

        [MaxLength(50)]
        public PaymentMethods PaymentMethod { get; set; } = PaymentMethods.Cash; // Default payment method

        // --- Relationships ---

        public virtual Job? Job { get; set; } // Navigation Property to the Job

    }
}
