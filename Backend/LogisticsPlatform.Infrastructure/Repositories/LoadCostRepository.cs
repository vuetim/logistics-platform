using LogisticsPlatform.Application.Interfaces.Repositories;
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

    public Task AddAsync(LoadCost cost)
    {
        return _context.LoadCosts.AddAsync(cost).AsTask();
    }
}
