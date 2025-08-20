using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.IUpgradeRecordRepository
{
    public interface IUpgradeRecordRepository : IRepository<UpgradeRecord>
    {


        Task AddUpgradeRecordAsync(UpgradeRecord upgraderRecord);
        Task UpdateUpgradeRecordWithComponentAsync(UpgradeRecord upradeRecord, List<UpgradeRecordComponent> componentstoAdd, List<int> componentIdstoRemove);
        Task<List<UpgradeRecord>> GetUpgradeRecordsByJobIdAsync(int jobId);
        Task<UpgradeRecord> GetUpgradeRecordByIdWithComponentsAsync(int upgradeRecordId);

        

    }
}
