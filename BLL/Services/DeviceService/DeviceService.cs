using Core.Common;
using Core.Enums;
using Core.IRepository;
using Core.IRepository.IDeviceImageRepository;
using Core.IRepository.IDeviceRepository;
using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.DeviceService
{
    public class DeviceService
    {
        private readonly IRepository<Device> _genericRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceImageRepository _deviceImageRepository;
        public DeviceService(IRepository<Device> genericRepository, IDeviceRepository deviceRepository, IDeviceImageRepository deviceImageRepository)
        {
            _genericRepository = genericRepository;
            _deviceRepository = deviceRepository;
            _deviceImageRepository = deviceImageRepository;
        }

        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            return (await _genericRepository.GetAll()).ToList();
        }

        public async Task<Device> GetDeviceById(int id)
        {
            return await _genericRepository.GetById(id);
        }

        public async Task AddDevice(int customerId, Device device)
        {
            try
            {
                LoggingService.LogInformation($"Adding device: {device.Brand} {device.Model} for customer {customerId}");


                // Validate the device object
                if (device == null)
                    throw new ArgumentNullException(nameof(device), "Device cannot be null");

                // Validate device properties
                if (string.IsNullOrWhiteSpace(device.Model))
                    throw new ArgumentException("Device model is required.", nameof(device.Model));

                if (device.Model.Length > 150)
                    throw new ArgumentException("Device model cannot exceed 150 characters.", nameof(device.Model));

                if (!string.IsNullOrEmpty(device.Brand) && device.Brand.Length > 100)
                    throw new ArgumentException("Device brand cannot exceed 100 characters.", nameof(device.Brand));

                if (!string.IsNullOrEmpty(device.SerialNumber) && device.SerialNumber.Length > 100)
                    throw new ArgumentException("Device serial number cannot exceed 100 characters.", nameof(device.SerialNumber));

                // Check for duplicate serial number if provided
                if (!string.IsNullOrWhiteSpace(device.SerialNumber))
                {
                    var existingDevices = await _genericRepository.GetAll();
                    if (existingDevices.Any(d => d.SerialNumber == device.SerialNumber && d.RecordStatus == RecordStatus.Active))
                    {
                        throw new ArgumentException("A device with this serial number already exists.");
                    }
                }

           


                await _deviceRepository.AddDevice(customerId, device);
                LoggingService.LogInformation($"Successfully added device: {device.DeviceId} - {device.Brand} {device.Model}");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to add device for customer {customerId}: {device?.Brand} {device?.Model}");
                throw;
            }
        }



        public async Task UpdateDevice(Device device)
        {
            await _genericRepository.Update(device);
        }

        public async Task DeleteDevice(int id)
        {
            try
            {
                LoggingService.LogInformation($"Deleting device: {id}");

                //get the device to ensure it exists
                var device = await GetDeviceById(id);

                //if the device is not null set the record status to cancelled
                if (device != null)
                {
                    device.RecordStatus = RecordStatus.Deleted;
                    await _deviceRepository.Update(device);
                    LoggingService.LogInformation($"Device {id} marked as Cancelled.");
                   
                }
                else
                {
                    LoggingService.LogWarning($"Device {id} not found for deletion.");
                    throw new KeyNotFoundException($"Device with ID {id} not found.");
                }

                //await _genericRepository.Delete(id);

               
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to delete device: {id}");
                throw;
            }
        }

        public async Task<List<Device>> GetDevicebyCustomerId(int customerId)
        {
            return await _deviceRepository.GetDevicebyCustomerId(customerId);

        }

        /// Add additional images to a device
        public async Task AddImages(int id, List<DeviceImage> images)
        {
            var device = await GetDeviceById(id);

            if (device == null)
            {
                throw new Exception("Device not found");
            }

            // ensures each image has the correct DeviceId before saving.
            foreach (var image in images)
            {
                image.DeviceId = id;
            }

            // Add images to the device
            await _deviceImageRepository.AddImages(images);
        }


        public async Task<Device> GetDeviceByIdWithImages(int id)
        {
            return await _deviceRepository.GetDevicebyIdwithImages(id);
        }

        public async Task<Device> UpdateDevice(Device devicetoupdate, List<DeviceImage> imagestoadd, List<int> imagestoremove)
        {
            try
            {
                LoggingService.LogInformation($"Updating device: {devicetoupdate.DeviceId} - {devicetoupdate.Brand} {devicetoupdate.Model}");

                return await _deviceRepository.UpdateDevice(devicetoupdate, imagestoadd, imagestoremove);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to update device: {devicetoupdate.DeviceId}");
                throw;
            }
        }


        public async Task<List<Device>> GetDeviceByRecordStatus(RecordStatus recordStatus)
        {
            return await _deviceRepository.GetDeviceByRecordStatus(recordStatus);
        }

        public async Task<List<Device>> GetDeviceByDeviceType(DeviceType deviceType)
        {
            return await _deviceRepository.GetDeviceByDeviceType(deviceType);
        }



        public async Task<string> GenerateNextBuildSerialNumberAsync()
        {
            string serialPrefix = "BUILD";
            // The service can call the repository to get the data
            var allDevices = await _deviceRepository.GetAll(); // Assuming you have this method

            var maxBuildNumber = allDevices
                .Where(d => d.SerialNumber != null && d.SerialNumber.StartsWith(serialPrefix))
                .Select(d => int.TryParse(d.SerialNumber.Substring(serialPrefix.Length), out int num) ? num : 0)
                .DefaultIfEmpty(0)
                .Max();

            int nextBuildNumber = maxBuildNumber + 1;
            return $"{serialPrefix}{nextBuildNumber}";
        }

        //search devices by customer name, device model, or serial number
        public async Task<List<Device>> SearchDevices(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Return an empty list immediately if search term is empty
                return new List<Device>();
            }

            // The query uses EF.Functions.Like for efficient, case-insensitive partial matching
            return await _deviceRepository.SearchDevices(searchTerm);
        }


        //a method to get all devices including their customer full name
        public async Task<List<Device>> GetAllDevicesWithCustomerFullName()
        {
            return await _deviceRepository.GetAllDevicesWithCustomerFullName();


        }


        //get recently added devices (last 30 days)
        public async Task<List<Device>> GetRecentDevicesAsync(int days)
        {
            if (days <= 0)
            {
                throw new ArgumentException("Days must be greater than zero.", nameof(days));
            }
            return await _deviceRepository.GetRecentDevicesAsync(days);
        }
    }
}
