using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class CustomerAddressRepository : ICustomerAddressRepository
{
    private readonly AppDbContext _context;

    public CustomerAddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerAddress>> GetByCustomerAsync(Guid customerId)
    {
        return await _context.CustomerAddresses
            .Where(a => a.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<CustomerAddress?> GetByIdAsync(Guid id)
    {
        return await _context.CustomerAddresses.FindAsync(id);
    }

    public async Task AddAsync(CustomerAddress address)
    {
        await _context.CustomerAddresses.AddAsync(address);
    }

    public async Task UpdateAsync(CustomerAddress address)
    {
        _context.CustomerAddresses.Update(address);
    }

    public async Task DeleteAsync(CustomerAddress address)
    {
        _context.CustomerAddresses.Remove(address);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
