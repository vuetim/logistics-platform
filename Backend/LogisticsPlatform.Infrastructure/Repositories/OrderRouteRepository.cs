using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class OrderRouteRepository : IOrderRouteRepository
{
    private readonly AppDbContext _context;

    public OrderRouteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OrderRoute route)
    {
        await _context.OrderRoutes.AddAsync(route);
    }

    public async Task<OrderRoute?> GetByIdAsync(Guid id)
    {
        return await _context.OrderRoutes
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<OrderRoute>> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.OrderRoutes
            .Where(r => r.OrderId == orderId && r.IsActive)
            .OrderBy(r => r.Sequence)
            .ToListAsync();
    }

    public Task UpdateAsync(OrderRoute route)
    {
        _context.OrderRoutes.Update(route);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(OrderRoute route)
    {
        route.IsActive = false; // ✅ soft delete
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
