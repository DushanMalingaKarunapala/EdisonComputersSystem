using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.IComponentRepository
{
    public interface IComponentRepository : IRepository<Component>
    {
        Task<List<Component>> GetComponentbytype(Core.Enums.ComponentType componentType);

        //Task<List<Component>> GetonlyAvailableComponents();

        // Retrieves all components of a specific type that are available in stock
        Task DecreseStockAsync(int componentId, int quantitytoDecrese);

        // Increases the stock of a component by a specified quantity
        Task IncreaseStockAsync(int componentId, int quantitytoIncrease);

        // Retrieves only available components of a specific type
        Task<IEnumerable<Component>> GetOnlyAvailableComponentsByTypeAsync(Core.Enums.ComponentType componentType);

        // Retrieves all components that are available in stock, regardless of type

        Task<IEnumerable<Component>> GetAllAvailableComponentsAsync();

        //filter repairrecordcomponent by component type
        Task<IEnumerable<Component>> GetComponentsByRepairRecordComponentTypeAsync(Core.Enums.ComponentType componentType);


        //filter components that are low in stock (e.g., less than 5 units)
        Task<List<LowinStockComponentsViewModel>> GetLowStockComponentsAsync(int threshold = 5);


        //a method to search components by name or serial number and return a list of components
        Task<List<Component>> SearchComponentsAsync(string searchTerm);


    }
}
