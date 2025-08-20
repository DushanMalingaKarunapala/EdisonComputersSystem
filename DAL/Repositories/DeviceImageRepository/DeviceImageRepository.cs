using Core.IRepository.IDeviceImageRepository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.DeviceImageRepository
{
    public class DeviceImageRepository : Repository<DeviceImage>, IDeviceImageRepository
    {
        private readonly ApplicationDbContext _context;
        public DeviceImageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddImages(List<DeviceImage> images)
        {
            await _context.DeviceImages.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }
        // Implement other methods from IRepository if needed
    }
    
}
