using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class DeviceViewModel
    {
        //public int CustomerId { get; set; }
        public int DeviceId { get; set; }
        public DeviceType DeviceType { get; set; }
        public string? Description { get; set; }
        public string? SerialNumber { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public DateTime RecievedDate { get; set; }
        public string? CustomerName { get; set; } 

        public List<DeviceImage> UploadedImages { get; set; } = new();

    }
}
