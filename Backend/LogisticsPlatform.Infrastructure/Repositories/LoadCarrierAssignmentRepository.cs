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
            .Include(x => x.Carrier)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<LoadCarrierAssignment>> GetByLoadIdAsync(Guid loadId)
    {
        return await _context.LoadCarrierAssignments
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

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
