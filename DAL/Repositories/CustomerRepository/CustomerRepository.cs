using Core.Enums;
using Core.IRepository.ICustomerRepository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.CustomerRepository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Customer>> GetCustomersByDeviceType(DeviceType deviceType)
        {
            return await _dbContext.Customers
                .Where(c => c.Devices.Any(d => d.DeviceType == deviceType))
                .ToListAsync();
        }

        public async  Task<List<Customer>> GetCustomersByRecordStatus(RecordStatus recordStatus)
        {
            //filter customers by record status
            return await _dbContext.Customers
                .Where(c => c.RecordStatus == recordStatus)
                .ToListAsync();
        }





        //get the count of new customers this month
        public async Task<int> GetNewCustomersThisMonthCountAsync()
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return await _dbContext.Customers
                .Where(c => c.Created >= startOfMonth)
                .CountAsync();

        }

        //a method to search customers by name, phone, or email and return a list of customers
        public async Task<List<Customer>> SearchCustomersAsync(string searchTerm)
        {

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Return an empty list immediately if search term is empty
                return new List<Customer>();
            }

            // Trim the search term once at the beginning
            searchTerm = searchTerm.Trim();

            // The query uses EF.Functions.Like for efficient, case-insensitive partial matching
            // and .Contains for the phone number which is generally faster for non-text columns.
            return await _dbContext.Customers
                .Where(c => EF.Functions.Like(c.FirstName, $"%{searchTerm}%") ||
                            EF.Functions.Like(c.LastName, $"%{searchTerm}%") ||
                            c.Phone.Contains(searchTerm) ||
                            (c.Email != null && EF.Functions.Like(c.Email, $"%{searchTerm}%")))
                .ToListAsync();
        }



        //public async Task<List<Customer>> GetFilteredCustomersAsync(Expression<Func<Customer, bool>> predicate)
        //{
        //    // Important: Include Devices if you need to filter by them!
        //    return await _dbContext.Customers
        //                           .Include(c => c.Devices)
        //                           .Where(predicate)
        //                           .ToListAsync();
        //}


    }
}
