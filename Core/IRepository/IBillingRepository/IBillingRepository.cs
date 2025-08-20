using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.IBillingRepository
{
    public interface IBillingRepository : IRepository<Billing>
    {
         
        Task<Billing> GetBillingByJobIdAsync(int jobId);
        Task<IEnumerable<Billing>> GetAllBillingsAsync();
        Task<IEnumerable<Billing>> GetBillingsByCustomerIdAsync(int customerId);
        Task<IEnumerable<Billing>> GetBillingsByDeviceIdAsync(int deviceId);
    }
}
