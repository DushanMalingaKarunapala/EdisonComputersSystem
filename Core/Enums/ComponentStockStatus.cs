using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    //an enum to represent the stock status and availability of a component
    public class ComponentStockStatus
    {
        public enum StockStatus
        {
            InStock, // Component is available in stock
            OutOfStock, // Component is not available in stock
            LowStock, // Component is running low on stock
            PreOrder, // Component can be pre-ordered
            Discontinued // Component is no longer available for sale
        }
        public static string GetStatusMessage(StockStatus status)
        {
            return status switch
            {
                StockStatus.InStock => "Component is available in stock.",
                StockStatus.OutOfStock => "Component is currently out of stock.",
                StockStatus.LowStock => "Component is running low on stock.",
                StockStatus.PreOrder => "Component can be pre-ordered.",
                StockStatus.Discontinued => "Component has been discontinued.",
                _ => "Unknown stock status."
            };
        }
    }
}
