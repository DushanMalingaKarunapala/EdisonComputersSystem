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

        public Task<bool> ExistsAsync(string phone, string? email = null, int? excludeCustomerId = null)
        {
            // Check if a customer exists with the given phone and optionally email, excluding a specific customer by ID
            var query = _dbContext.Customers.AsQueryable();

            // Exclude the current customer if updating
            if (excludeCustomerId.HasValue)
            {
                query = query.Where(c => c.CustomerId != excludeCustomerId.Value);
            }

            // Check if phone exists OR email exists (if email is provided)
            if (!string.IsNullOrWhiteSpace(email))
            {
                // Check for duplicate phone OR duplicate email
                return query.Where(c => c.Phone == phone || c.Email == email).AnyAsync();
            }
            else
            {
                // Only check for duplicate phone
                return query.Where(c => c.Phone == phone).AnyAsync();
            }

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
                .Where(c => c.RecordStatus!= RecordStatus.Deleted &&  c.Created >= startOfMonth) //count only active customers not deleted
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



        public async Task<List<Customer>> GetRecentlyAddedCustomersAsync(int months)
        {
            var cutoffDate = DateTime.UtcNow.AddMonths(-months);// Use UtcNow for consistency
            return await _dbContext.Customers
                .Where(c => c.RecordStatus == RecordStatus.Active &&
                           (c.Created >= cutoffDate ||
                            c.Devices.Any(d => d.Jobs.Any(j => j.CheckedInDate >= cutoffDate))))
                .Include(c => c.Devices)
                .OrderByDescending(c => c.Created)
                .Take(30)
                .ToListAsync();
        }




    }
}
