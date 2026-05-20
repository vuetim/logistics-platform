using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Services.Orders;

public class OrderDetailsMapper : IOrderDetailsMapper
{
    public OrderDetailsDto Map(Order order)
        => new()
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CustomerId = order.CustomerId,
            Status = order.Status,
            Phase = order.Phase,
            StartDate = BuildDateMeta(order.StartDate),
            EndDate = BuildDateMeta(order.EndDate),
            StartDateType = new LookupValueDto { Key = "33091", Value = "On a specific date" },
            EndDateType = new LookupValueDto { Key = "33091", Value = "On a specific date" },
            PlannedPickup = order.PlannedPickupDate.HasValue ? BuildDateMeta(order.PlannedPickupDate.Value) : null,
            PlannedDelivery = order.PlannedDeliveryDate.HasValue ? BuildDateMeta(order.PlannedDeliveryDate.Value) : null,
            DispatchNotes = order.DispatchNotes,
            DeliveryNotes = order.DeliveryNotes,
            CustomerRate = order.CustomerRate
        };

    private static OrderDateDto BuildDateMeta(DateTime date)
        => new()
        {
            Date = date,
            Timezone = "UTC",
            HasTime = true
        };
}

