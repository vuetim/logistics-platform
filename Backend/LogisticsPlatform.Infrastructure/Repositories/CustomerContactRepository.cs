using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<CustomerContact>> GetPrimaryByCustomerAsync(Guid customerId)
    {
        return await _context.CustomerContacts
            .Where(x => x.CustomerId == customerId && x.IsPrimary)
            .ToListAsync();
    }

    public async Task AddAsync(CustomerContact contact)
    {
        await _context.CustomerContacts.AddAsync(contact);
    }

    public void Update(CustomerContact contact)
    {
        _context.CustomerContacts.Update(contact);
    }

    public void Remove(CustomerContact contact)
    {
        _context.CustomerContacts.Remove(contact);
    }
}
