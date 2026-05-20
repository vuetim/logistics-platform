using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services.Loads.OrderToLoad;

public class LoadCostSnapshotBuilder : ILoadCostSnapshotBuilder
{
    public LoadCostSnapshot Build(Order order)
    {
        var orderCostLines = order.Cost?.LineItems.ToList() ?? new List<OrderCostLineItem>();
        var customerCostLines = orderCostLines
            .Where(x => x.IsCustomer)
            .ToList();

        var customerRate = customerCostLines
            .Where(x => x.Type == ChargeType.Linehaul)
            .Sum(x => x.Amount);

        if (customerRate <= 0)
            customerRate = order.CustomerRate ?? 0;

        var accessorials = customerCostLines
            .Where(x => x.Type != ChargeType.Linehaul)
            .Sum(x => x.Amount);

        var loadCostLines = orderCostLines
            .Where(x => x.Type != ChargeType.Linehaul)
            .Select(x => new LoadCostLineItem
            {
                Type = x.Type,
                Qty = x.Qty,
                Price = x.Price,
                Amount = x.Amount,
                IsCustomer = x.IsCustomer,
                IsCarrier = x.IsCarrier,
                Payable = x.IsCarrier,
                Notes = x.Notes
            })
            .ToList();

        var cost = loadCostLines.Any()
            ? new LoadCost
            {
                Notes = order.Cost?.Notes,
                TotalAmount = loadCostLines
                    .Where(x => x.IsCarrier)
                    .Sum(x => x.Amount),
                LineItems = loadCostLines
            }
            : null;

        return new LoadCostSnapshot(customerRate, accessorials, cost);
    }
}

