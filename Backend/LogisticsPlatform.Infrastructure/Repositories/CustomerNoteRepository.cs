using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class CustomerNoteRepository : ICustomerNoteRepository
{
    private readonly AppDbContext _context;

    public CustomerNoteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerNote>> GetByCustomerAsync(Guid customerId)
    {
        return await _context.CustomerNotes
            .Where(n => n.CustomerId == customerId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<CustomerNote?> GetByIdAsync(Guid id)
    {
        return await _context.CustomerNotes
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task AddAsync(CustomerNote note)
    {
        await _context.CustomerNotes.AddAsync(note);
    }

    public void Update(CustomerNote note)
    {
        _context.CustomerNotes.Update(note);
    }

    public void Remove(CustomerNote note)
    {
        _context.CustomerNotes.Remove(note);
    }
}
