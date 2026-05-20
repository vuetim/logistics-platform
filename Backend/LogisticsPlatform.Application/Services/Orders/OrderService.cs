using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orders;
    private readonly IOrderDatePolicy _datePolicy;
    private readonly IOrderCancellationPolicy _cancellationPolicy;
    private readonly IOrderDetailsMapper _detailsMapper;
    private readonly IOrderNumberGenerator _numberGenerator;

    public OrderService(
        IOrderRepository orders,
        IOrderDatePolicy datePolicy,
        IOrderCancellationPolicy cancellationPolicy,
        IOrderDetailsMapper detailsMapper,
        IOrderNumberGenerator numberGenerator)
    {
        _orders = orders;
        _datePolicy = datePolicy;
        _cancellationPolicy = cancellationPolicy;
        _detailsMapper = detailsMapper;
        _numberGenerator = numberGenerator;
    }

    public async Task<Guid> CreateAsync(CreateOrderDto dto, Guid userId)
    {
        _datePolicy.Validate(dto.StartDate?.Date, dto.EndDate?.Date, dto.PlannedPickup?.Date, dto.PlannedDelivery?.Date);

        var order = Order.Create(
            number: _numberGenerator.Generate(),
            customerId: dto.CustomerId,
            createdByUserId: userId,
            type: dto.OrderType,
            direction: dto.Direction,
            start: dto.StartDate!.Date,
            end: dto.EndDate!.Date,
            preferredCarrierId: dto.PreferredCarrierId
        );

        order.UpdatePlanning(dto.PlannedPickup?.Date, dto.PlannedDelivery?.Date);
        order.UpdateNotes(dto.DispatchNotes, dto.DeliveryNotes);
        order.SetCustomerRate(dto.CustomerRate ?? 0m);
        order.SetBusinessDetails(
            dto.PrimaryPONumber,
            dto.PrimaryBolNumber,
            dto.PrimaryProNumber,
            dto.Commodity,
            dto.TotalWeight,
            dto.TotalPallets,
            dto.TotalVolume);

        await _orders.AddAsync(order);
        await _orders.SaveChangesAsync();

        return order.Id;
    }

    public async Task UpdateAsync(Guid id, UpdateOrderDto dto)
    {
        var order = await _orders.GetByIdAsync(id)
            ?? throw new Exception("Order not found");

        _datePolicy.Validate(dto.StartDate?.Date, dto.EndDate?.Date, dto.PlannedPickup?.Date, dto.PlannedDelivery?.Date);

        order.UpdateCore(
            dto.OrderType,
            dto.Direction,
            dto.StartDate?.Date,
            dto.EndDate?.Date,
            dto.PreferredCarrierId);

        order.UpdatePlanning(dto.PlannedPickup?.Date, dto.PlannedDelivery?.Date);
        order.UpdateNotes(dto.DispatchNotes, dto.DeliveryNotes);
        order.SetBusinessDetails(
            dto.PrimaryPONumber,
            dto.PrimaryBolNumber,
            dto.PrimaryProNumber,
            dto.Commodity,
            dto.TotalWeight,
            dto.TotalPallets,
            dto.TotalVolume);

        if (dto.CustomerRate.HasValue)
            order.SetCustomerRate(dto.CustomerRate.Value);

        await _orders.UpdateAsync(order);
        await _orders.SaveChangesAsync();
    }

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

            _cancellationPolicy.EnsureCanCancel(loads);

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

    public async Task<OrderDetailsDto?> GetDetailsAsync(Guid id)
    {
        var order = await _orders.GetByIdAsync(id);
        return order == null ? null : _detailsMapper.Map(order);
    }
}
