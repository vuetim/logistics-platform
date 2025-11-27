using LogisticsPlatform.Application.Interfaces.Repositories;
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

    public async Task AddAsync(Load load)
    {
        await _context.Loads.AddAsync(load);
    }

    public async Task UpdateAsync(Load load)
    {
        _context.Loads.Update(load);
    }

    public async Task<Load?> GetByIdAsync(Guid id)
    {
        return await _context.Loads
            .Include(x => x.Customer)
            .Include(x => x.Carrier)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
