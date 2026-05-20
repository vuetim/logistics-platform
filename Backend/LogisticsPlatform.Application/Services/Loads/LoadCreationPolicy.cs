using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services.Loads;

public class LoadCreationPolicy : ILoadCreationPolicy
{
    public void ValidateDateOverrides(DateTime? plannedPickup, DateTime? plannedDelivery)
    {
        if (plannedPickup.HasValue &&
            plannedDelivery.HasValue &&
            plannedDelivery.Value < plannedPickup.Value)
        {
            throw new BusinessRuleException("Planned delivery cannot be before planned pickup.");
        }
    }

    public void EnsureCanCreateFromOrder(Order order, Order orderWithLoads, CreateLoadFromOrderDto dto)
    {
        if (order.Status == OrderStatus.Draft)
            throw new BusinessRuleException("Order must be submitted before creating a load.");

        if (orderWithLoads.Loads.Any(l => l.Load != null && !l.Load.IsArchived) && !dto.SplitOrder)
            throw new BusinessRuleException("Order already has an active load. Use split flow if needed.");

        if (dto.SplitOrder)
            throw new BusinessRuleException("SplitOrder is not implemented yet.");
    }
}

