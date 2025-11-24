using LogisticsPlatform.Application.Interfaces;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class CustomerContactRepository : ICustomerContactRepository
    {
        private readonly AppDbContext _context;

        public CustomerContactRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerContact>> GetByCustomerAsync(Guid customerId)
        {
            return await _context.CustomerContacts
                .Where(c => c.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<CustomerContact?> GetByIdAsync(Guid id)
        {
            return await _context.CustomerContacts
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(CustomerContact contact)
        {
            await _context.CustomerContacts.AddAsync(contact);
        }

        public async Task UpdateAsync(CustomerContact contact)
        {
            _context.CustomerContacts.Update(contact);
        }

        public async Task DeleteAsync(CustomerContact contact)
        {
            _context.CustomerContacts.Remove(contact);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
