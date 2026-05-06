using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orders;

    public OrderService(IOrderRepository orders)
    {
        _orders = orders;
    }

    // CREATE
   
    public async Task<Guid> CreateAsync(CreateOrderDto dto, Guid userId)
    {
        var order = Order.Create(
            number: GenerateNumber(),
            customerId: dto.CustomerId,
            createdByUserId: userId,
            type: dto.OrderType,
            direction: dto.Direction,
            start: dto.StartDate.Date,
            end: dto.EndDate.Date,
            preferredCarrierId: dto.PreferredCarrierId
        );

        order.UpdatePlanning(
            dto.PlannedPickup?.Date,
            dto.PlannedDelivery?.Date
        );
        order.UpdateNotes(dto.DispatchNotes, dto.DeliveryNotes);
        order.SetCustomerRate(dto.CustomerRate ?? 0m);
        order.SetBusinessDetails(dto.PrimaryPONumber, dto.PrimaryBolNumber,
            dto.PrimaryProNumber, dto.Commodity, dto.TotalWeight, dto.TotalPallets, dto.TotalVolume);

        await _orders.AddAsync(order);
        await _orders.SaveChangesAsync();

        return order.Id;
    }
    // UPDATE

    public async Task UpdateAsync(Guid id, UpdateOrderDto dto)
    {
        var order = await _orders.GetByIdAsync(id)
            ?? throw new Exception("Order not found");

        order.UpdateCore(
            dto.OrderType,
            dto.Direction,
            dto.StartDate?.Date,
            dto.EndDate?.Date,
            dto.PreferredCarrierId
        );

        order.UpdatePlanning(
            dto.PlannedPickup?.Date,
            dto.PlannedDelivery?.Date
        );
        order.UpdateNotes(dto.DispatchNotes, dto.DeliveryNotes);
        order.SetBusinessDetails(
            dto.PrimaryPONumber,
            dto.PrimaryBolNumber,
            dto.PrimaryProNumber,
            dto.Commodity,
            dto.TotalWeight,
            dto.TotalPallets,
            dto.TotalVolume
        );

        if (dto.CustomerRate.HasValue)
            order.SetCustomerRate(dto.CustomerRate.Value);

        await _orders.UpdateAsync(order);
        await _orders.SaveChangesAsync();
    }

    // STATUS

    public async Task ChangeStatusAsync(Guid id, OrderStatus status)
    {
        if (status == OrderStatus.Cancelled)
        {
            var orderWithLoads = await _orders.GetByIdWithLoadsAsync(id)
                ?? throw new Exception("Order not found");

            var loads = orderWithLoads.Loads
                .Where(x => x.Load != null)
                .Select(x => x.Load!)
                .ToList();

            if (loads.Any(IsExecutionOrHigherLoad))
            {
                throw new InvalidOperationException(
                    "Order cannot be cancelled because a linked load is in execution or already completed.");
            }

            foreach (var load in loads.Where(l => l.Status != LoadStatus.Cancelled))
            {
                load.Status = LoadStatus.Cancelled;
            }

            orderWithLoads.ChangeStatus(OrderStatus.Cancelled);
            await _orders.SaveChangesAsync();
            return;
        }

        var order = await _orders.GetByIdAsync(id)
            ?? throw new Exception("Order not found");

        order.ChangeStatus(status);

        await _orders.SaveChangesAsync();
    }

    // DETAILS (for Angular wizard)

    public async Task<OrderDetailsDto?> GetDetailsAsync(Guid id)
    {
        var order = await _orders.GetByIdAsync(id);
        if (order == null) return null;

        return new OrderDetailsDto
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
    }

    private static OrderDateDto BuildDateMeta(DateTime date)
        => new()
        {
            Date = date,
            Timezone = "UTC",
            HasTime = true
        };

    private static string GenerateNumber()
        => $"O-{DateTime.UtcNow:yyyyMMddHHmmss}";

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
