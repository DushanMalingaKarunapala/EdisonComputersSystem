using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IService
{
    public interface ICustomerService
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<List<Customer>> GetAllCustomersAsync();
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
        Task<List<Customer>> GetCustomersByDeviceTypeAsync(DeviceType deviceType);
    }
}
