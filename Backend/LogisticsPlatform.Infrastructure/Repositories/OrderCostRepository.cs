using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class OrderCostRepository : IOrderCostRepository
{
    private readonly AppDbContext _context;

    public OrderCostRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<OrderCost?> GetByOrderIdAsync(Guid orderId)
    {
        return _context.OrderCosts
            .Include(c => c.LineItems)
            .FirstOrDefaultAsync(c => c.OrderId == orderId);
    }

    public Task AddAsync(OrderCost cost)
    {
        return _context.OrderCosts.AddAsync(cost).AsTask();
    }
}
