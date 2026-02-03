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

    public async Task<List<CustomerAddress>> GetPrimaryByCustomerAsync(Guid customerId)
    {
        return await _context.CustomerAddresses
            .Where(x => x.CustomerId == customerId && x.IsPrimary)
            .ToListAsync();
    }

    public async Task AddAsync(CustomerAddress address)
    {
        await _context.CustomerAddresses.AddAsync(address);
    }

    public void Update(CustomerAddress address)
    {
        _context.CustomerAddresses.Update(address);
    }

    public void Remove(CustomerAddress address)
    {
        _context.CustomerAddresses.Remove(address);
    }
}
