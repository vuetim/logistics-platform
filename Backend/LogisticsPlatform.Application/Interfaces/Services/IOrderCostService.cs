using LogisticsPlatform.Application.DTOs.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface IOrderCostService
    {
        Task<OrderCostDto> GetAsync(Guid orderId);
        Task UpdateAsync(Guid orderId, UpdateOrderCostDto dto, Guid userId);
    }
}
