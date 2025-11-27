using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories;

public class LoadEquipmentRepository : ILoadEquipmentRepository
{
    private readonly AppDbContext _context;

    public LoadEquipmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LoadEquipment equipment)
    {
        await _context.LoadEquipment.AddAsync(equipment);
    }

    public Task UpdateAsync(LoadEquipment equipment)
    {
        _context.LoadEquipment.Update(equipment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoadEquipment equipment)
    {
        _context.LoadEquipment.Remove(equipment);
        return Task.CompletedTask;
    }

    public async Task<LoadEquipment?> GetByIdAsync(Guid id)
    {
        return await _context.LoadEquipment.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<LoadEquipment>> GetByLoadIdAsync(Guid loadId)
    {
        return await _context.LoadEquipment
            .Where(x => x.LoadId == loadId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
