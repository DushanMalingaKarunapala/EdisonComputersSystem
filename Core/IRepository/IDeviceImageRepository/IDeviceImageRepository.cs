using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.IDeviceImageRepository
{
    public interface IDeviceImageRepository : IRepository<DeviceImage>
    {
        Task AddImages(List<DeviceImage> images);
    }
}
