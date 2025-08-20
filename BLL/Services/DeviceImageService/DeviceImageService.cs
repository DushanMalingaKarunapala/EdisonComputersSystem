using Core.IRepository;
using Core.IRepository.IDeviceImageRepository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.DeviceImageService
{
    public class DeviceImageService
    {
        private readonly IDeviceImageRepository _deviceImageRepository;
        public DeviceImageService(IDeviceImageRepository deviceImageRepository)
        {
            _deviceImageRepository = deviceImageRepository;
        }


        public async Task AddImages(List<DeviceImage> images)
        {
            await _deviceImageRepository.AddImages(images);
        }

        public async Task<List<DeviceImage>> GetImagesByDeviceId(int deviceId)
        {
            return (List<DeviceImage>)await _deviceImageRepository.GetAll();
        }

        public async Task<DeviceImage> GetImageById(int id)
        {
            return await _deviceImageRepository.GetById(id);
        }

        public async Task DeleteImage(int id)
        {
            await _deviceImageRepository.Delete(id);
        }
    }
}
