using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services.Orders;

public class OrderCancellationPolicy : IOrderCancellationPolicy
{
    public void EnsureCanCancel(IEnumerable<Load> linkedLoads)
    {
        if (linkedLoads.Any(IsExecutionOrHigherLoad))
        {
            throw new InvalidOperationException(
                "Order cannot be cancelled because a linked load is in execution or already completed.");
        }
    }

    private static bool IsExecutionOrHigherLoad(Load load)
    {
        if (load.Status == LoadStatus.Cancelled)
            return false;

        return load.Status switch
        {
            LoadStatus.Dispatched => true,
            LoadStatus.EnRouteToPickup => true,
            LoadStatus.AtPickup => true,
            LoadStatus.Loaded => true,
            LoadStatus.EnRouteToDelivery => true,
            LoadStatus.AtDelivery => true,
            LoadStatus.InTransit => true,
            LoadStatus.Delivered => true,
            LoadStatus.Completed => true,
            _ => false
        };
    }
}

