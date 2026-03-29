using Core.Enums;
using Core.IRepository.IDeviceRepository;
using Core.Models;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.DeviceRepository
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DeviceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddDevice(int customerId, Device device)
        {
            var customer = await _dbContext.Customers.FindAsync(customerId);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with {customerId} not found!");
            }

            device.CustomerId = customerId; // set the customer id

            await _dbContext.Devices.AddAsync(device);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Device>> GetDevicebyCustomerId(int customerId)
        {
            return await _dbContext.Devices.Where(d => d.CustomerId == customerId).ToListAsync();
        }

       

        public async Task<Device> GetDevicebyIdwithImages(int id)
        {
            var device = await _dbContext.Devices
                .Include(d => d.DeviceImages)
                .Include(d => d.Customer) // Include customer details if needed
                .FirstOrDefaultAsync(d => d.DeviceId == id);
            if (device == null)
            {
                throw new KeyNotFoundException($"Device with ID {id} not found!");
            }
            return device;
        }

        



        //define the method to update device with images
        public async Task<Device> UpdateDevice(Device devicetoupdate, List<DeviceImage> imagestoadd, List<int> imagestoremove)
        {
            //fetch the existing device with images from the database
            var existingDevice = await _dbContext.Devices
                .Include(d => d.DeviceImages)
                .FirstOrDefaultAsync(d => d.DeviceId == devicetoupdate.DeviceId);

            if(existingDevice == null)
            {
                // If the device does not exist, throw an exception
                throw new KeyNotFoundException($"Device with ID {devicetoupdate.DeviceId} not found.");
            }

            //update the scalar properties of the device
            // Use SetValues to copy only scalar properties from deviceToUpdate to existingDevice
            // This avoids issues if deviceToUpdate is a different instance.
            _dbContext.Entry(existingDevice).CurrentValues.SetValues(devicetoupdate);

            //handle image removals
            if (imagestoremove != null && imagestoremove.Count > 0)
            {
                foreach (var imageId in imagestoremove)
                {
                    var imageToRemove = existingDevice.DeviceImages.FirstOrDefault(img => img.Id == imageId);
                    if (imageToRemove != null)
                    {
                        _dbContext.DeviceImages.Remove(imageToRemove);

                        //delete the image file from the disk if it exists
                        var imagePath = Path.Combine("C:\\Users\\Asus\\Downloads", imageToRemove.ImagePath); // Adjust the path as necessary
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }
                }
            }

            //handle image additions
            if (imagestoadd != null && imagestoadd.Count > 0)
            {
                foreach (var image in imagestoadd)
                {
                    // Ensure the image has a valid path before adding
                    if (!string.IsNullOrEmpty(image.ImagePath))
                    {
                        existingDevice.DeviceImages.Add(image);
                    }
                }
            }

            // Save changes to the database
            await _dbContext.SaveChangesAsync();
            return existingDevice; // Return the updated device with images
        }


        public async Task<List<Device>> GetDeviceByDeviceType(DeviceType deviceType)
        {
            return await _dbContext.Devices
                .Include(d => d.Customer) // Include customer details
                .Where(d => d.DeviceType == deviceType)
                .ToListAsync();

        }


        public async Task<List<Device>> GetDeviceByRecordStatus(RecordStatus recordStatus)
        {
            return await _dbContext.Devices
                .Include(d => d.Customer) // Include customer details  
                .Where(d => d.RecordStatus == recordStatus)
                .ToListAsync();

        }

        //a method to search devices by customer name, device model, or serial number
        public Task<List<Device>> SearchDevices(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Return an empty list immediately if search term is empty
                return Task.FromResult(new List<Device>());
            }

            //trim the search term to avoid leading/trailing spaces
            searchTerm = searchTerm.Trim();


            // The query uses EF.Functions.Like for efficient, case-insensitive partial matching
            // and .Contains for the phone number which is generally faster for non-text columns.
            return _dbContext.Devices
                .Include(d => d.Customer) // Include customer details
                .Where(d => EF.Functions.Like(d.Model, $"%{searchTerm}%") ||
                            EF.Functions.Like(d.SerialNumber, $"%{searchTerm}%") ||
                            EF.Functions.Like(d.Customer.FirstName, $"%{searchTerm}%") || // Search by customer first name
                            EF.Functions.Like(d.Customer.LastName, $"%{searchTerm}%")) // Search by customer last name
                .ToListAsync();


        }

        public async Task<List<Device>> GetAllDevicesWithCustomerFullName()
        {
            //fetch all devices including only the customer full name
            return await _dbContext.Devices
                .Include(d => d.Customer) // Include customer details
             
                .ToListAsync();



        }

        public async Task<List<Device>> GetRecentDevicesAsync(int days)
        {

            var cutoffDate = DateTime.Now.AddDays(-days);
            return await _dbContext.Devices
                .Include(d => d.Customer)
                .Where(d => d.RecordStatus == RecordStatus.Active && d.ReceivedDate >= cutoffDate)//get the active devices that were received in the last specified number of days
                .OrderByDescending(d => d.ReceivedDate)
                .ToListAsync();
        }
    }
}
