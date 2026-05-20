using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories;

public class LoadRepository : ILoadRepository
{
    private readonly AppDbContext _context;

    public LoadRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddLoadOrderAsync(LoadOrder link)
    {
        await _context.LoadOrders.AddAsync(link);
    }

    public async Task AddAsync(Load load)
    {
        await _context.Loads.AddAsync(load);
    }
    public async Task AddStopAsync(LoadStop stop)
    {
        await _context.LoadStops.AddAsync(stop);
    }

    public async Task UpdateAsync(Load load)
    {
        _context.Loads.Update(load);
    }

    public async Task<Load?> GetByIdAsync(Guid id)
    {
        return await _context.Loads
            .Include(l => l.Customer)
            .Include(l => l.Carrier)
            .Include(l => l.Cost)
                .ThenInclude(c => c.LineItems)
            .Include(l => l.Stops)
            .Include(l => l.Orders)
                .ThenInclude(lo => lo.Order)
            .FirstOrDefaultAsync(l => l.Id == id);
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
