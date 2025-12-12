using LogisticsPlatform.Application.DTOs.Costs;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories.Financial;

public class LoadFinancialSnapshotRepository : ILoadFinancialSnapshotRepository
{
    private readonly AppDbContext _context;

    public LoadFinancialSnapshotRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LoadCostSummaryDto> GetSnapshotAsync(Guid loadId)
    {
        var load = await _context.Loads
            .Include(l => l.Cost)
                .ThenInclude(c => c.LineItems)
            .Include(l => l.Orders)
                .ThenInclude(o => o.Order)
                    .ThenInclude(o => o.Cost)
                        .ThenInclude(c => c.LineItems)
            .FirstOrDefaultAsync(l => l.Id == loadId);

        if (load == null)
            throw new Exception("Load not found.");

        var billable = (load.CustomerRate ?? 0)
            + (load.Cost?.LineItems
                    .Where(x => x.IsCustomer)
                    .Sum(x => x.Amount) ?? 0);

        var payable = (load.CarrierRate ?? 0)
            + (load.Cost?.LineItems
                    .Where(x => x.IsCarrier)
                    .Sum(x => x.Amount) ?? 0);

        return new LoadCostSummaryDto
        {
            CustomerRate = load.CustomerRate ?? 0,
            CarrierRate = load.CarrierRate ?? 0,
            Margin = billable - payable,
            TotalBillable = billable,
            TotalPayable = payable
        };
    }
}
