using Core.IRepository;
using Core.IRepository.ICustomerRepository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.CustomerService
{
    public class CustomerService
    {

        private readonly ICustomerRepository _customerRepository;

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
            if (string.IsNullOrWhiteSpace(customer.FirstName))
            {
                throw new ArgumentException("Customer first name cannot be null or empty.");
            }
            if (customer.Phone == null || customer.Phone.Length < 10)
            {
                throw new ArgumentException("Customer phone number must be at least 10 characters long.");
            }
            if (customer.Email != null && !customer.Email.Contains("@"))
            {
                throw new ArgumentException("Customer email must be a valid email address.");
            }

            await _customerRepository.Add(customer);

        }

        public async Task UpdateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName))
            {
                throw new ArgumentException("Customer first name cannot be null or empty.");
            }
            if (customer.Phone == null || customer.Phone.Length < 10)
            {
                throw new ArgumentException("Customer phone number must be at least 10 characters long.");
            }
            if (customer.Email != null && !customer.Email.Contains("@"))
            {
                throw new ArgumentException("Customer email must be a valid email address.");
            }
            await _customerRepository.Update(customer);
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
            var customer = await _customerRepository.GetById(id);
            if (customer != null)
            {
                customer.RecordStatus = Core.Enums.RecordStatus.Deleted; // Assuming Deleted is the status for cancellation
                await _customerRepository.Update(customer);
            }
            else
            {
                throw new ArgumentException($"Customer with ID {id} does not exist.");
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


    }
}
