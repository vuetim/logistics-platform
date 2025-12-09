using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Repositories
{
    public interface IOrderCostRepository
    {
        Task<OrderCost?> GetByOrderIdAsync(Guid orderId);
        Task AddAsync(OrderCost cost);
        //Task SaveChangesAsync();
    }
}
