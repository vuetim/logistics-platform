using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services.Loads.OrderToLoad;

public class OrderToLoadRouteSelector : IOrderToLoadRouteSelector
{
    public SelectedOrderRoutes Select(Order order)
    {
        var activeRoutes = order.OrderRoutes
            .Where(r => r.IsActive)
            .OrderBy(r => r.Sequence)
            .ToList();

        var mandatoryPickup = activeRoutes
            .Where(r => r.StopType == StopType.Pickup)
            .OrderBy(r => r.Sequence)
            .FirstOrDefault();

        var mandatoryDelivery = activeRoutes
            .Where(r => r.StopType == StopType.Delivery)
            .OrderByDescending(r => r.Sequence)
            .FirstOrDefault();

        if (mandatoryPickup == null || mandatoryDelivery == null)
            throw new BusinessRuleException("Order must have at least one pickup and one delivery route to create a load.");

        var mandatoryRouteIds = new[]
            {
                mandatoryPickup.Id,
                mandatoryDelivery.Id
            }
            .ToHashSet();

        var routes = activeRoutes
            .Where(r => r.CopyToLoad || mandatoryRouteIds.Contains(r.Id))
            .OrderBy(r => r.Sequence)
            .ToList();

        if (!routes.Any())
            throw new BusinessRuleException("No active routes to copy.");

        return new SelectedOrderRoutes(routes);
    }
}

