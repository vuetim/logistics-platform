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

    public Task<OrderCost?> GetByOrderIdForUpdateAsync(Guid orderId)
    {
        return _context.OrderCosts
            .FirstOrDefaultAsync(c => c.OrderId == orderId);
    }

    public Task DeleteLineItemsByOrderCostIdAsync(Guid orderCostId)
    {
        return _context.OrderCostLineItems
            .Where(li => li.OrderCostId == orderCostId)
            .ExecuteDeleteAsync();
    }

    public Task AddLineItemsAsync(IEnumerable<OrderCostLineItem> lineItems)
    {
        return _context.OrderCostLineItems.AddRangeAsync(lineItems);
    }

    public Task AddAsync(OrderCost cost)
    {
        return _context.OrderCosts.AddAsync(cost).AsTask();
    }
}
