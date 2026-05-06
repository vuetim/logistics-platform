using LogisticsPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Orders
{
    public interface IOrderCostRepository
    {
        Task<OrderCost?> GetByOrderIdAsync(Guid orderId);
        Task<OrderCost?> GetByOrderIdForUpdateAsync(Guid orderId);
        Task DeleteLineItemsByOrderCostIdAsync(Guid orderCostId);
        Task AddLineItemsAsync(IEnumerable<OrderCostLineItem> lineItems);
        Task AddAsync(OrderCost cost);
        //Task SaveChangesAsync();
    }
}
