using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class DeviceImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string ImagePath { get; set; }
    
        public int DeviceId { get; set; } // Foreign Key to the Device
        public Device? Device { get; set; }
    }
}
