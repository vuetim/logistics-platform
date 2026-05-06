using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class OrderNoteRepository : IOrderNoteRepository
    {
        private readonly AppDbContext _context;

        public OrderNoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderNote>> GetByOrderAsync(Guid orderId)
        {
            return await _context.OrderNotes
                .Include(n => n.CreatedByUser)
                .Where(n => n.OrderId == orderId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<OrderNote?> GetByIdAsync(Guid id)
        {
            return await _context.OrderNotes
                .Include(n => n.CreatedByUser)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task AddAsync(OrderNote note)
        {
            await _context.OrderNotes.AddAsync(note);
        }

        public void Update(OrderNote note)
        {
            _context.OrderNotes.Update(note);
        }

        public void Remove(OrderNote note)
        {
            _context.OrderNotes.Remove(note);
        }
    }
}
