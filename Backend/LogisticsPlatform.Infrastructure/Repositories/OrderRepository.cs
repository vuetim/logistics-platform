using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.OrderRoutes)
                .Include(o => o.Items)
                .Include(o => o.Cost)
                .Include(o => o.EquipmentRequirements)
                .Include(o => o.ExternalIds)
                .Include(o => o.Notes)
                .Include(o => o.Documents)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Order?> GetByIdWithRoutesAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.OrderRoutes)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
