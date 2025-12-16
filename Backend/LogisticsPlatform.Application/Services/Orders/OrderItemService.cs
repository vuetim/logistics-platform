using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using SendGrid.Helpers.Errors.Model;

namespace LogisticsPlatform.Application.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderRepository _orders;
    private readonly IOrderItemRepository _items;


    public OrderItemService(IOrderRepository orders, IOrderItemRepository items)
    {
        _orders = orders;
        _items = items;
    }

    public async Task<Guid> AddAsync(Guid orderId, CreateOrderItemDto dto, Guid userId)
    {
        var order = await _orders.GetByIdAsync(orderId)
            ?? throw new NotFoundException("Order not found.");

        if (order.Status != OrderStatus.Draft)
            throw new BusinessRuleException("Items can be added only in Draft orders.");

        var item = new OrderItem
        {
            OrderId = orderId,
            Name = dto.Name,
            CustomerReference = dto.CustomerReference,

            Quantity = dto.Quantity,
            QuantityUnit = dto.QuantityUnit,

            IsHazmat = dto.IsHazmat,
            HazardClass = dto.HazardClass,
            IdentificationNumber = dto.IdentificationNumber,

            FreightClass = dto.FreightClass,
            Notes = dto.Notes


        };

        await _items.AddAsync(item);
        await _orders.SaveChangesAsync();

        return item.Id;
    }

    public async Task<List<OrderItemDto>> GetByOrderIdAsync(Guid orderId)
    {
        var order = await _orders.GetByIdAsync(orderId)
            ?? throw new NotFoundException("Order not found.");

        return order.Items.Select(i => new OrderItemDto
        {
            Id = i.Id,
            Name = i.Name,
            CustomerReference = i.CustomerReference,
            IdentificationNumber = i.IdentificationNumber,
            Quantity = i.Quantity,
            QuantityUnit = i.QuantityUnit,
            IsHazmat = i.IsHazmat,
            FreightClass = i.FreightClass,
            HazardClass = i.HazardClass,
            Notes = i.Notes
        }).ToList();
    }
}
