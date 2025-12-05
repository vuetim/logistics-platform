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

    public async Task<LoadItem?> GetByIdAsync(Guid loadId, Guid itemId)
    {
        return await _context.LoadItems
            .FirstOrDefaultAsync(i => i.Id == itemId && i.LoadId == loadId);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
