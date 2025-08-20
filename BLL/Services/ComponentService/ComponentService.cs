using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ComponentService
{
    public class ComponentService
    {
        private readonly Core.IRepository.IComponentRepository.IComponentRepository _componentRepository;
        public ComponentService(Core.IRepository.IComponentRepository.IComponentRepository componentRepository)
        {
            _componentRepository = componentRepository;
        }
        public async Task<List<Core.Models.Component>> GetComponentsByType(Core.Enums.ComponentType componentType)
        {
            return await _componentRepository.GetComponentbytype(componentType);
        }

        public async Task<Core.Models.Component> GetComponentById(int id)
        {
            return await _componentRepository.GetById(id);
        }
        public async Task AddComponentAsync(Core.Models.Component component)
        {
            if (component.Price < 0)
            {
                throw new ArgumentException("Component price cannot be negative.");
            }
            if(string.IsNullOrWhiteSpace(component.Name))
            {
                throw new ArgumentException("Component name cannot be null or empty.");
            }
            if(component.QuantityInStock < 0)
            {
                throw new ArgumentException("Component quantity in stock cannot be negative.");
            }
            await _componentRepository.Add(component);
        }

        public async Task UpdateComponent(Core.Models.Component component)
        {
            if (component.Price < 0)
            {
                throw new ArgumentException("Component price cannot be negative.");
            }
            if (string.IsNullOrWhiteSpace(component.Name))
            {
                throw new ArgumentException("Component name cannot be null or empty.");
            }
            if (component.QuantityInStock < 0)
            {
                throw new ArgumentException("Component quantity in stock cannot be negative.");
            }
            await _componentRepository.Update(component);
        }

        public async Task DeleteComponent(int id)
        {

            await _componentRepository.Delete(id);
        }
        public async Task<IEnumerable<Core.Models.Component>> GetAllComponentsAsync()
        {
            return (await _componentRepository.GetAll()).ToList();
        }

        public async Task<IEnumerable<Core.Models.Component>> GetOnlyAvailableComponentsAsync()
        {
            return await _componentRepository.GetAllAvailableComponentsAsync();
        }


        //filter repairrecordcomponent by component type
        public async Task<IEnumerable<Core.Models.Component>> GetComponentsByRepairRecordComponentTypeAsync(Core.Enums.ComponentType componentType)
        {
            return await _componentRepository.GetComponentsByRepairRecordComponentTypeAsync(componentType);
        }


        //cancel the component stock status update
        //cancel the customer by setting the record status to cancelled
        public async Task CancelComponent(int id)
        {
            var component = await _componentRepository.GetById(id);
            if (component != null)
            {
                //component.com = Core.Enums.RecordStatus.Deleted; // Assuming Deleted is the status for cancellation
                component.StockStatus = Core.Enums.ComponentStockStatus.StockStatus.Discontinued; // Assuming Cancelled is a valid stock status
                await _componentRepository.Update(component);
            }
            else
            {
                throw new ArgumentException($"Component with ID {id} does not exist.");
            }
        }


        //search components by name or serial number and return a list of components
        public async Task<List<Core.Models.Component>> SearchComponentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Return an empty list immediately if search term is empty
                return new List<Core.Models.Component>();
            }
            return await _componentRepository.SearchComponentsAsync(searchTerm);
        }


    }
}
