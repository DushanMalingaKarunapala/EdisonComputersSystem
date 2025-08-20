using Core.Enums;
using Core.IRepository.IBillingRepository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.BillingRepository
{

    public class BillingRepository : Repository<Billing>, IBillingRepository
    {
        private readonly ApplicationDbContext _context;

        public BillingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<Billing>> GetAllBillingsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Billing> GetBillingByJobIdAsync(int jobId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Billing>> GetBillingsByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Billing>> GetBillingsByDeviceIdAsync(int deviceId)
        {
            throw new NotImplementedException();
        }

       
    }
}
