using Core.IRepository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.BillingService
{
    public class BillingService
    {
        private readonly IRepository<Billing> _billingRepository;
        public BillingService(IRepository<Billing> billingRepository)
        {
            _billingRepository = billingRepository;
        }


        public async Task<IEnumerable<Billing>> GetAllBillings()
        {
            return (await _billingRepository.GetAll()).ToList();
        }

        public async Task<Billing> GetBillingById(int id)
        {
            return await _billingRepository.GetById(id);
        }
        public async Task AddBilling(Billing billing)
        {
            await _billingRepository.Add(billing);
        }

        public async Task UpdateBilling(Billing billing)
        {
            await _billingRepository.Update(billing);
        }

        public async Task DeleteBilling(int id)
        {
            await _billingRepository.Delete(id);
        }
    }
}
