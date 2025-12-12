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

    public async Task AddAsync(CarrierSettlement settlement)
    {
        await _context.CarrierSettlements.AddAsync(settlement);
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
