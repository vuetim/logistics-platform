using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services.Orders;

public class OrderLoadSyncService : IOrderLoadSyncService
{
    private readonly IOrderRepository _orders;

    public OrderLoadSyncService(IOrderRepository orders)
    {
        _orders = orders;
    }

    public async Task SyncFromLoadAsync(Load load)
    {
        if (load.Orders == null || load.Orders.Count == 0)
            return;

        var orderIds = load.Orders
            .Select(x => x.OrderId)
            .Distinct()
            .ToList();

        var hasChanges = false;
        foreach (var orderId in orderIds)
        {
            hasChanges = await SyncOrderInternalAsync(orderId) || hasChanges;
        }

        if (hasChanges)
            await _orders.SaveChangesAsync();
    }

    public async Task SyncByOrderIdAsync(Guid orderId)
    {
        if (await SyncOrderInternalAsync(orderId))
            await _orders.SaveChangesAsync();
    }

    private async Task<bool> SyncOrderInternalAsync(Guid orderId)
    {
        var order = await _orders.GetByIdWithLoadsAsync(orderId);
        if (order == null)
            return false;

        var target = ResolveTargetStatus(order.Loads
            .Where(x => x.Load != null)
            .Select(x => x.Load.Status)
            .ToList());

        if (!target.HasValue)
            return false;

        return order.TrySyncStatusFromExecution(target.Value);
    }

    private static OrderStatus? ResolveTargetStatus(List<LoadStatus> statuses)
    {
        if (statuses.Count == 0)
            return null;

        var active = statuses.Where(s => s != LoadStatus.Cancelled).ToList();

        if (active.Count == 0)
            return OrderStatus.Cancelled;

        if (active.All(s => s == LoadStatus.Completed))
            return OrderStatus.ReadyForBilling;

        if (active.All(s => s == LoadStatus.Delivered || s == LoadStatus.Completed))
            return OrderStatus.Delivered;

        if (active.Any(s => s == LoadStatus.AtDelivery))
            return OrderStatus.AtDelivery;

        if (active.Any(s =>
            s == LoadStatus.InTransit ||
            s == LoadStatus.EnRouteToDelivery ||
            s == LoadStatus.Loaded))
            return OrderStatus.InTransit;

        if (active.Any(s => s == LoadStatus.AtPickup))
            return OrderStatus.AtPickup;

        if (active.Any(s =>
            s == LoadStatus.Dispatched ||
            s == LoadStatus.EnRouteToPickup))
            return OrderStatus.Dispatched;

        if (active.Any(s =>
            s == LoadStatus.Accepted ||
            s == LoadStatus.Tendered ||
            s == LoadStatus.Planned))
            return OrderStatus.Scheduled;

        if (active.Any(s =>
            s == LoadStatus.Draft ||
            s == LoadStatus.Rejected))
            return OrderStatus.Confirmed;

        return null;
    }
}
