using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.Interfaces.Services.Orders;

namespace LogisticsPlatform.Application.Services.Orders;

public class OrderDatePolicy : IOrderDatePolicy
{
    public void Validate(DateTime? start, DateTime? end, DateTime? plannedPickup, DateTime? plannedDelivery)
    {
        if (start.HasValue && end.HasValue && end.Value < start.Value)
            throw new BusinessRuleException("Order window end cannot be before order window start.");

        if (plannedPickup.HasValue && plannedDelivery.HasValue && plannedDelivery.Value < plannedPickup.Value)
            throw new BusinessRuleException("Planned delivery cannot be before planned pickup.");

        if (start.HasValue && plannedPickup.HasValue && plannedPickup.Value < start.Value)
            throw new BusinessRuleException("Planned pickup cannot be before order window start.");

        if (end.HasValue && plannedDelivery.HasValue && plannedDelivery.Value > end.Value)
            throw new BusinessRuleException("Planned delivery cannot be after order window end.");
    }
}

