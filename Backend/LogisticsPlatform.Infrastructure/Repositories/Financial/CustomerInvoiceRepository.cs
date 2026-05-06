using LogisticsPlatform.Domain.Entities.Financial;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories.Financial
{
    public class CustomerInvoiceRepository : ICustomerInvoiceRepository
    {
        private readonly AppDbContext _context;

        public CustomerInvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<CustomerInvoice?> GetByLoadIdAsync(Guid loadId)
        {
            return _context.CustomerInvoices
                .Include(i => i.LineItems)
                .Include(i => i.Customer)
                .Include(i => i.Load)
                    .ThenInclude(l => l.Items)
                .Include(i => i.Load)
                    .ThenInclude(l => l.Stops)
                .FirstOrDefaultAsync(i => i.LoadId == loadId);
        }

        public Task<CustomerInvoice?> GetByIdAsync(Guid id)
        {
            return _context.CustomerInvoices
                .Include(i => i.LineItems)
                .Include(i => i.Customer)
                .Include(i => i.Load)
                    .ThenInclude(l => l.Items)
                .Include(i => i.Load)
                    .ThenInclude(l => l.Stops)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<List<CustomerInvoice>> ListAsync()
        {
            return _context.CustomerInvoices
                .Include(i => i.LineItems)
                .Include(i => i.Customer)
                .Include(i => i.Load)
                .OrderByDescending(i => i.InvoiceDate)
                .ToListAsync();
        }

        public Task DeleteLineItemsByInvoiceIdAsync(Guid invoiceId)
        {
            return _context.CustomerInvoiceLineItems
                .Where(li => li.InvoiceId == invoiceId)
                .ExecuteDeleteAsync();
        }

        public Task AddLineItemsAsync(IEnumerable<CustomerInvoiceLineItem> lineItems)
        {
            return _context.CustomerInvoiceLineItems.AddRangeAsync(lineItems);
        }

        public async Task AddAsync(CustomerInvoice invoice)
        {
            await _context.CustomerInvoices.AddAsync(invoice);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
