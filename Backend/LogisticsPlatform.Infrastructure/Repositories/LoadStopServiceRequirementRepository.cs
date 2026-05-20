using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class LoadStopServiceRequirementRepository : ILoadStopServiceRequirementRepository
{
    private readonly AppDbContext _context;

    public LoadStopServiceRequirementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<LoadStopServiceRequirement>> GetByStopAsync(Guid stopId)
    {
        return await _context.LoadStopServiceRequirements
            .AsNoTracking()
            .Where(x => x.LoadStopId == stopId)
            .OrderBy(x => x.ServiceValue)
            .ToListAsync();
    }

    public async Task AddAsync(LoadStopServiceRequirement requirement)
    {
        await _context.LoadStopServiceRequirements.AddAsync(requirement);
    }

    public async Task<LoadStopServiceRequirement?> GetByIdForStopWithLoadAsync(Guid stopId, Guid serviceId)
    {
        return await _context.LoadStopServiceRequirements
            .Include(x => x.LoadStop)
                .ThenInclude(x => x.Load)
            .FirstOrDefaultAsync(x => x.Id == serviceId && x.LoadStopId == stopId);
    }

    public Task DeleteAsync(LoadStopServiceRequirement requirement)
    {
        _context.LoadStopServiceRequirements.Remove(requirement);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
