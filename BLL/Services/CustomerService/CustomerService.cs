using Core.Common;
using Core.IRepository;
using Core.IRepository.ICustomerRepository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Services.CustomerService
{
    public class CustomerService
    {

        private readonly ICustomerRepository _customerRepository;
        private static readonly Regex EmailRegex = new Regex(
           @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
           RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return (await _customerRepository.GetAll()).ToList();
        }


        public async Task<Customer> GetCustomerById(int id)
        {
            return await _customerRepository.GetById(id);
        }

        public async Task AddCustomer(Customer customer)
        {
            try
            {
                LoggingService.LogInformation($"Adding new customer: {customer.FirstName} {customer.LastName}");

                //validate the customer before adding
                await ValidateCustomer(customer);

                //check for the duplicate customer by phone and email
                if (await _customerRepository.ExistsAsync(customer.Phone, customer.Email))
                {
                    LoggingService.LogWarning($"Duplicate customer attempt: {customer.Phone}, {customer.Email}");

                    throw new ArgumentException("A customer with the same phone or email already exists.");
                }

              


                //finally add the customer to the repository

                await _customerRepository.Add(customer);

                LoggingService.LogInformation($"Successfully added customer: {customer.FirstName} {customer.LastName} (ID: {customer.CustomerId})");

            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to add customer: {customer.FirstName} {customer.LastName}");
                throw;

            }
        }

        public async Task UpdateCustomer(Customer customer)
        {
            try
            {
                LoggingService.LogInformation($"Updating customer: {customer.CustomerId} - {customer.FirstName} {customer.LastName}");

                //validate the customer before updating
                await ValidateCustomer(customer);

                //check for the duplicate customer by phone and email, excluding the current customer
                if (await _customerRepository.ExistsAsync(customer.Phone, customer.Email, customer.CustomerId))
                {
                    LoggingService.LogWarning($"Duplicate customer data during update: {customer.CustomerId}");

                    throw new ArgumentException("A customer with the same phone or email already exists.");
                }

                //set the updated date
                customer.Updated = DateTime.UtcNow;



                await _customerRepository.Update(customer);
                LoggingService.LogInformation($"Successfully updated customer: {customer.CustomerId}");

            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to update customer: {customer.CustomerId}");
                throw;
            }
        }



        public async Task DeleteCustomer(int id)
        {
            await _customerRepository.Delete(id);
        }

        public async Task<List<Customer>> FilterCustomersByDeviceType(Core.Enums.DeviceType deviceType)
        {
            return await _customerRepository.GetCustomersByDeviceType(deviceType);
        }

        public async Task<List<Customer>> GetCustomersByRecordStatus(Core.Enums.RecordStatus recordStatus)
        {
            return await _customerRepository.GetCustomersByRecordStatus(recordStatus);
        }

        //public async Task<List<Customer>> GetFilteredCustomersAsync(Expression<Func<Customer, bool>> predicate)
        //{
        //    return await _customerRepository.GetFilteredCustomersAsync(predicate);
        //}

        //cancel the customer by setting the record status to cancelled
        public async Task CancelCustomer(int id)
        {
            try
            {
                LoggingService.LogInformation($"Cancelling customer: {id}");

                var customer = await _customerRepository.GetById(id);
                if (customer != null)
                {
                    customer.RecordStatus = Core.Enums.RecordStatus.Deleted; // Assuming Deleted is the status for cancellation
                    await _customerRepository.Update(customer);

                    LoggingService.LogInformation($"Successfully cancelled customer: {id} - {customer.FirstName} {customer.LastName}");

                }
                else
                {
                    LoggingService.LogWarning($"Attempted to cancel non-existent customer: {id}");

                    throw new ArgumentException($"Customer with ID {id} does not exist.");
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to cancel customer: {id}");
                throw;
            }
        }


        //search customers by name, phone, or email
        public async Task<List<Customer>> SearchCustomersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Customer>();
            }
            return await _customerRepository.SearchCustomersAsync(searchTerm);
        }



        //get the recent customers added this month
        public async Task<List<Customer>> GetRecentActiveCustomersAsync(int months)
        {
            return await _customerRepository.GetRecentlyAddedCustomersAsync(months);
        }








        //validate the customer properties
        private async Task ValidateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (string.IsNullOrWhiteSpace(customer.FirstName))
                throw new ArgumentException("First name is required.", nameof(customer.FirstName));

            if (customer.FirstName.Length > 200)
                throw new ArgumentException("First name cannot exceed 200 characters.", nameof(customer.FirstName));

            if (!string.IsNullOrEmpty(customer.LastName) && customer.LastName.Length > 200)
                throw new ArgumentException("Last name cannot exceed 200 characters.", nameof(customer.LastName));

            if (string.IsNullOrWhiteSpace(customer.Phone))
                throw new ArgumentException("Phone number is required.", nameof(customer.Phone));

            if (!IsValidPhoneNumber(customer.Phone))
                throw new ArgumentException("Phone number must be at least 10 digits and contain only valid characters.", nameof(customer.Phone));

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                if (customer.Email.Length > 255)
                    throw new ArgumentException("Email cannot exceed 255 characters.", nameof(customer.Email));

                if (!IsValidEmail(customer.Email))
                    throw new ArgumentException("Please provide a valid email address.", nameof(customer.Email));
            }

            if (!string.IsNullOrEmpty(customer.Address) && customer.Address.Length > 500)
                throw new ArgumentException("Address cannot exceed 500 characters.", nameof(customer.Address));
        }

        //validate phone number and email format
        private static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Remove common formatting characters
            var cleanPhone = Regex.Replace(phone, @"[\s\-\(\)\+]", "");

            // Must have at least 10 digits
            return cleanPhone.Length >= 10 && Regex.IsMatch(cleanPhone, @"^\d+$");
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }



    }
}
