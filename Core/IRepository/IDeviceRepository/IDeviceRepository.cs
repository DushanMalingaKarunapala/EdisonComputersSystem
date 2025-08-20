using Core.Enums;
using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.IDeviceRepository
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Task AddDevice(int customerId, Device device);
        Task<List<Device>> GetDevicebyCustomerId(int customerId);
        Task <Device> GetDevicebyIdwithImages(int id);
        Task <Device> UpdateDevice(Device devicetoupdate, List<DeviceImage> imagestoadd, List<int> imagestoremove);

        //get device by record status
        Task<List<Device>> GetDeviceByRecordStatus(RecordStatus recordStatus);
        //get device by device type
        Task<List<Device>> GetDeviceByDeviceType(DeviceType deviceType);

        //a method to search devices by customer name, device model, or serial number
        Task<List<Device>> SearchDevices(string searchTerm);

        //a method to get all devices including theier customer full name
        Task<List<Device>> GetAllDevicesWithCustomerFullName();
    }
}
