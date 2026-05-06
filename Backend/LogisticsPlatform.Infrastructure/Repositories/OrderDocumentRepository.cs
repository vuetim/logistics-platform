using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class OrderDocumentRepository : IOrderDocumentRepository
    {
        private readonly AppDbContext _context;

        public OrderDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDocument>> GetByOrderAsync(Guid orderId)
        {
            return await _context.OrderDocuments
                .Where(d => d.OrderId == orderId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<OrderDocument?> GetByIdAsync(Guid id)
        {
            return await _context.OrderDocuments
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(OrderDocument document)
        {
            await _context.OrderDocuments.AddAsync(document);
        }

        public void Remove(OrderDocument document)
        {
            _context.OrderDocuments.Remove(document);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
