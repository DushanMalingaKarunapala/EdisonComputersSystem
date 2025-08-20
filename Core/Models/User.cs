
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required] // Example Data Annotation
        [MaxLength(100)]
        public required string Username { get; set; } 

        [Required]
        public required string PasswordHash { get; set; } // Store securely hashed passwords only

        [MaxLength(255)]
        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Add Role if needed for separating Admins/Technicians
        // public Role role{ get; set; } // e.g., "Admin", "Technician"

        // Relationships (Inverse Navigation Properties)
        //public virtual ICollection<RepairRecord> AssignedRepairs { get; set; } = new List<RepairRecord>();
    }
}
