using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.IServiceRecordRepository
{
    public interface IServiceRecordRepository : IRepository<ServiceRecord>
    {
        Task AddServiceRecordAsync(ServiceRecord serviceRecord);
        Task UpdateServiceRecord(ServiceRecord serviceRecord);
        Task<List<ServiceRecord>> GetServiceRecordsByJobIdAsync(int jobId);
    }
}
