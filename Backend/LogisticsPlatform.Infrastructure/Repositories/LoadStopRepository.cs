using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories;

public class LoadStopRepository : ILoadStopRepository
{
    private readonly AppDbContext _context;

    public LoadStopRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LoadStop stop)
    {
        _context.LoadStops.Add(stop);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(LoadStop stop)
    {
        _context.LoadStops.Update(stop);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(LoadStop stop)
    {
        _context.LoadStops.Remove(stop);
        await _context.SaveChangesAsync();
    }

    public async Task<LoadStop?> GetByIdAsync(Guid id)
    {
        return await _context.LoadStops.FindAsync(id);
    }

    public async Task<List<LoadStop>> GetByLoadIdAsync(Guid loadId)
    {
        return await _context.LoadStops
            .Where(s => s.LoadId == loadId)
            .OrderBy(s => s.Sequence)
            .ToListAsync();
    }
}
