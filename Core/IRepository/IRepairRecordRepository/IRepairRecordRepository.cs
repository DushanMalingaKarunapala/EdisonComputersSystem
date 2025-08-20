using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.IRepairRecordRepository
{
    public interface IRepairRecordRepository : IRepository<RepairRecord>
    {
        Task AddRepairRecordAsync(RepairRecord repairRecord);
        Task UpdateRepairRecordWithComponentAsync(RepairRecord repairRecord, List<RepairRecordComponent> componentstoAdd, List<int> componentIdstoRemove);
        
        Task<RepairRecord> GetRepairRecordByIdWithComponentsAsync(int repairRecordId);
        Task<List<RepairRecord>> GetRepairRecordsByJobIdWithJobAsync(int jobId); // New method

        //Task<List<RepairRecord>> GetRepairRecordsByDeviceId(int deviceId);

        //Task<List<RepairRecord>> GetRepairRecordsByDeviceType(DeviceType deviceType);
        //Task<RepairRecord> GetRepairRecordById(int id);
        //Task DeleteRepairRecord(int id);
    }

}
