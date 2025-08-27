using Core.Enums;
using Core.Models;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.ComponentRepository
{
    public class ComponentRepository : Repository<Component>, Core.IRepository.IComponentRepository.IComponentRepository
    {
        private readonly ApplicationDbContext _context;
        public ComponentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task DecreseStockAsync(int componentId, int quantitytoDecrese)
        {

            if (quantitytoDecrese <= 0) return; // Or throw ArgumentOutOfRangeException

            var component = await _context.Components.FindAsync(componentId);
            if (component == null)
            {
                throw new KeyNotFoundException($"Component with ID {componentId} not found.");
            }
            if (component.QuantityInStock < quantitytoDecrese)
            {
                // This check should ideally happen in the service BEFORE calling this,
                // but a final check here is good.
                throw new InvalidOperationException($"Insufficient stock for component {component.Name}. Required: {quantitytoDecrese}, Available: {component.QuantityInStock}.");
            }
            component.QuantityInStock -= quantitytoDecrese;
            // _dbContext.Entry(component).State = EntityState.Modified; // Not strictly needed if already tracked
            await _context.SaveChangesAsync(); // Save immediately or as part of a larger Unit of Work

        }



        public async Task<IEnumerable<Component>> GetAllAvailableComponentsAsync()
        {
            return await _context.Components.Where(c => c.QuantityInStock > 0 ).ToListAsync(); // Assuming soft delete
        }



        public async Task<List<Component>> GetComponentbytype(ComponentType componentType)
        {
            
            return await _context.Components
                .Where(c=> c.Type == componentType)
                .ToListAsync();
        }

        
        //public Task<List<Component>> GetonlyAvailableComponents()
        //{
        //    //get all the components that are available in stock more than 0 quantity
        //    return _context.Components
        //        .Where(c => c.QuantityInStock > 0)
        //        .ToListAsync();
        //}

        public Task<IEnumerable<Component>> GetOnlyAvailableComponentsByTypeAsync(ComponentType componentType)
        {
            throw new NotImplementedException();
        }

        public async Task IncreaseStockAsync(int componentId, int quantitytoIncrease)
        {
            if (quantitytoIncrease <= 0) return;

            var component = await _context.Components.FindAsync(componentId);
            if (component == null)
            {
                throw new KeyNotFoundException($"Component with ID {componentId} not found.");
            }
            component.QuantityInStock += quantitytoIncrease;
            // _dbContext.Entry(component).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // Save immediately or as part of a larger Unit of Work
        }


        public async Task<IEnumerable<Component>> GetComponentsByRepairRecordComponentTypeAsync(ComponentType componentType)
        {
            //filter repairrecordcomponents by component type
            return await _context.Components
                .Where(r => r.Type == componentType)
                .ToListAsync();
        }


        /// <summary>
        /// a method to get components that are low in stock
        public async Task<List<LowinStockComponentsViewModel>> GetLowStockComponentsAsync(int threshold = 5)
        {
            //filter components that are low in stock (e.g., less than 5 units)
            return await _context.Components
                .Where(c => c.QuantityInStock < threshold)
                .Select(c => new LowinStockComponentsViewModel
                {
                    ComponentId = c.ComponentId,
                    SerialNumber = c.SerialNumber,
                    ComponentName = c.Name,
                    ComponentType = c.Type,
                    QuantityInStock = c.QuantityInStock

                })
                .ToListAsync();

        }

        public Task<List<Component>> SearchComponentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Return an empty list immediately if search term is empty
                return Task.FromResult(new List<Component>());
            }

            //trim the search term to avoid leading/trailing spaces
            searchTerm = searchTerm.Trim();

            // The query uses EF.Functions.Like for efficient, case-insensitive partial matching
            // and .Contains for the phone number which is generally faster for non-text columns.
            return _context.Components
                .Where(c => EF.Functions.Like(c.Name, $"%{searchTerm}%") || 
                            EF.Functions.Like(c.SerialNumber, $"%{searchTerm}%"))
                .ToListAsync();


        }

        public async Task<List<Component>> GetPriorityComponentsAsync()
        {
            return await _context.Components
                .Where(c => c.QuantityInStock < 5 || // Low stock
                           c.AddedDate >= DateTime.Now.AddDays(-30)) // Recently added
                .OrderBy(c => c.QuantityInStock) // Low stock first
                .ThenByDescending(c => c.AddedDate)
                .Take(20) // Limit to top 10 priority components
                .ToListAsync();

        }
    }
}
