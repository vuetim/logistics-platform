using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Infrastructure.Migrations;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class LoadCarrierAssignmentRepository : ILoadCarrierAssignmentRepository
{
    private readonly AppDbContext _context;

    public LoadCarrierAssignmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LoadCarrierAssignment assignment)
    {
        await _context.LoadCarrierAssignments.AddAsync(assignment);
    }

    public async Task UpdateAsync(LoadCarrierAssignment assignment)
    {
        _context.LoadCarrierAssignments.Update(assignment);
    }

    public async Task<LoadCarrierAssignment?> GetByIdAsync(Guid id)
    {
        return await _context.LoadCarrierAssignments
            .Include(x => x.Load)
                .ThenInclude(x => x.Stops)
            .Include(x => x.Carrier)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<LoadCarrierAssignment?> GetByTenderTokenAsync(string token)
    {
        return await _context.LoadCarrierAssignments
            .Include(x => x.Load)
                .ThenInclude(x => x.Customer)
            .Include(x => x.Load)
                .ThenInclude(x => x.Stops)
            .Include(x => x.Carrier)
            .FirstOrDefaultAsync(x => x.TenderToken == token);
    }

    public async Task<IEnumerable<LoadCarrierAssignment>> GetByLoadIdAsync(Guid loadId)
    {
        return await _context.LoadCarrierAssignments
            .Include(x => x.Carrier)
            .Where(x => x.LoadId == loadId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<LoadCarrierAssignment?> GetActiveByLoadAsync(Guid loadId)
    {
        return await _context.LoadCarrierAssignments
            .Where(x =>
                x.LoadId == loadId &&
                x.IsActive &&
                x.Status == AssignmentStatus.Tendered)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<LoadCarrierAssignment>> GetOpenTenderedAsync()
    {
        return await _context.LoadCarrierAssignments
            .AsNoTracking()
            .Include(x => x.Carrier)
            .Include(x => x.Load)
                .ThenInclude(x => x.Customer)
            .Where(x =>
                x.IsActive &&
                x.Status == AssignmentStatus.Tendered &&
                x.Load.Status == LoadStatus.Tendered)
            .OrderBy(x => x.TenderExpiresAt ?? DateTime.MaxValue)
            .ThenByDescending(x => x.TenderedAt)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
