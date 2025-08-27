using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.ICustomerRepository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        
        Task<List<Customer>> GetCustomersByDeviceType(DeviceType deviceType);


        //get customers by record status
        Task<List<Customer>> GetCustomersByRecordStatus(RecordStatus recordStatus);

        Task<int> GetNewCustomersThisMonthCountAsync();

        // Use Expression Trees for dynamic filtering
        //Task<List<Customer>> GetFilteredCustomersAsync(Expression<Func<Customer, bool>> predicate);

        //a method to search customers by name, phone, or email
        Task<List<Customer>> SearchCustomersAsync(string searchTerm);


        //a method to check if a customer exists by phone and optionally by email
        Task<bool> ExistsAsync(string phone, string? email = null, int? excludeCustomerId = null);


        //a method to get recently added customers within a specified months
        Task<List<Customer>> GetRecentlyAddedCustomersAsync(int months);
    }
}
