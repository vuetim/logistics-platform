using LogisticsPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Loads
{
    public interface ILoadCostRepository
    {
        Task<LoadCost?> GetByLoadIdAsync(Guid loadId);
        Task<LoadCost?> GetByLoadIdForUpdateAsync(Guid loadId);
        Task DeleteLineItemsByLoadCostIdAsync(Guid loadCostId);
        Task AddLineItemsAsync(IEnumerable<LoadCostLineItem> lineItems);
        Task AddAsync(LoadCost cost);
        //Task SaveChangesAsync();
    }
}
