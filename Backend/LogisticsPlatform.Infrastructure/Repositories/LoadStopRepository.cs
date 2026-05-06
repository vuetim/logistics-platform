using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class LoadStopRepository : ILoadStopRepository
{
    private readonly AppDbContext _context;

    public LoadStopRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(LoadStop stop)
    {
        _context.LoadStops.Add(stop);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(LoadStop stop)
    {
        _context.LoadStops.Update(stop);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoadStop stop)
    {
        _context.LoadStops.Remove(stop);
        return Task.CompletedTask;
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

    public async Task<LoadStop?> GetByIdWithLoadAsync(Guid id)
    {
        return await _context.LoadStops
            .Include(s => s.Load)
                .ThenInclude(l => l.Stops)
            .Include(s => s.Load)
                .ThenInclude(l => l.Orders)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<LoadStop>> GetEnRouteStopsWithLoadAsync()
    {
        return await _context.LoadStops
            .Include(s => s.Load)
            .Where(s => s.Status == StopStatus.EnRoute)
            .ToListAsync();
    }



}
