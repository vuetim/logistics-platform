using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class OrderExternalIdRepository : IOrderExternalIdRepository
    {
        private readonly AppDbContext _context;

        public OrderExternalIdRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderExternalId>> GetByOrderAsync(Guid orderId)
        {
            return await _context.OrderExternalIds
                .Where(x => x.OrderId == orderId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<OrderExternalId?> GetByIdAsync(Guid id)
        {
            return await _context.OrderExternalIds.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(OrderExternalId externalId)
        {
            await _context.OrderExternalIds.AddAsync(externalId);
        }

        public void Update(OrderExternalId externalId)
        {
            _context.OrderExternalIds.Update(externalId);
        }

        public void Remove(OrderExternalId externalId)
        {
            _context.OrderExternalIds.Remove(externalId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
