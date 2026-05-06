using LogisticsPlatform.Domain.Entities.Financial;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class CarrierSettlementRepository : ICarrierSettlementRepository
{
    private readonly AppDbContext _context;

    public CarrierSettlementRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<CarrierSettlement?> GetByLoadIdAsync(Guid loadId)
    {
        return _context.CarrierSettlements
            .Include(s => s.LineItems)
            .Include(s => s.Carrier)
            .Include(s => s.Load)
                .ThenInclude(l => l.Stops)
            .Include(s => s.Load)
                .ThenInclude(l => l.Items)
            .FirstOrDefaultAsync(s => s.LoadId == loadId);
    }

    public Task<CarrierSettlement?> GetByIdAsync(Guid id)
    {
        return _context.CarrierSettlements
            .Include(s => s.LineItems)
            .Include(s => s.Carrier)
            .Include(s => s.Load)
                .ThenInclude(l => l.Stops)
            .Include(s => s.Load)
                .ThenInclude(l => l.Items)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public Task<List<CarrierSettlement>> ListAsync()
    {
        return _context.CarrierSettlements
            .Include(s => s.LineItems)
            .Include(s => s.Carrier)
            .Include(s => s.Load)
            .OrderByDescending(s => s.SettlementDate)
            .ToListAsync();
    }

    public Task DeleteLineItemsBySettlementIdAsync(Guid settlementId)
    {
        return _context.CarrierSettlementLineItems
            .Where(li => li.SettlementId == settlementId)
            .ExecuteDeleteAsync();
    }

    public Task AddLineItemsAsync(IEnumerable<CarrierSettlementLineItem> lineItems)
    {
        return _context.CarrierSettlementLineItems.AddRangeAsync(lineItems);
    }

    public async Task AddAsync(CarrierSettlement settlement)
    {
        await _context.CarrierSettlements.AddAsync(settlement);
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
