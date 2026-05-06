using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories;

public class LoadCostRepository : ILoadCostRepository
{
    private readonly AppDbContext _context;

    public LoadCostRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<LoadCost?> GetByLoadIdAsync(Guid loadId)
    {
        return _context.LoadCosts
            .Include(c => c.LineItems)
            .FirstOrDefaultAsync(c => c.LoadId == loadId);
    }

    public Task<LoadCost?> GetByLoadIdForUpdateAsync(Guid loadId)
    {
        return _context.LoadCosts
            .FirstOrDefaultAsync(c => c.LoadId == loadId);
    }

    public Task DeleteLineItemsByLoadCostIdAsync(Guid loadCostId)
    {
        return _context.LoadCostLineItems
            .Where(li => li.LoadCostId == loadCostId)
            .ExecuteDeleteAsync();
    }

    public Task AddLineItemsAsync(IEnumerable<LoadCostLineItem> lineItems)
    {
        return _context.LoadCostLineItems.AddRangeAsync(lineItems);
    }

    public Task AddAsync(LoadCost cost)
    {
        return _context.LoadCosts.AddAsync(cost).AsTask();
    }
}
