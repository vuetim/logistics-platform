using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class LoadItemRepository : ILoadItemRepository
{
    private readonly AppDbContext _context;

    public LoadItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LoadItem item)
    {
        await _context.LoadItems.AddAsync(item);
    }

    public async Task<LoadItem?> GetByIdAsync(Guid loadId, Guid itemId)
    {
        return await _context.LoadItems
            .FirstOrDefaultAsync(i => i.Id == itemId && i.LoadId == loadId);
    }
    public async Task<IEnumerable<LoadItem>> GetByLoadIdAsync(Guid loadId)
    {
        return await _context.LoadItems
            .Where(x => x.LoadId == loadId)
            .ToListAsync();
    }
    public async Task DeleteAsync(LoadItem item)
    {
        _context.LoadItems.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
