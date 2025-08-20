using Core.IRepository.IJobRepository;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.InvoiceService
{
    public class InvoiceService
    {
        private readonly IjobRepository _jobRepository;

        public InvoiceService(IjobRepository jobRepository)
        {
         _jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
        }


        //public async Task<InvoiceViewModel> GetInvoiceViewModelAsync(int jobId)
        //{
        //    // Delegate to the repository
        //    return await _jobRepository.GetInvoiceDataAsync(jobId);
        //}
    }
}
