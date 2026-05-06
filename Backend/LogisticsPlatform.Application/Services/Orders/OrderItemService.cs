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

        if (!CanEditItems(order.Status))
            throw new BusinessRuleException("Items cannot be changed in this order status.");

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
            HandlingQuantity = dto.HandlingQuantity ?? dto.ActualQuantity,
            HandlingUnit = dto.HandlingUnit,
            UnitNetWeight = dto.UnitNetWeight,
            UnitGrossWeight = dto.UnitGrossWeight,
            WeightUnit = dto.WeightUnit,
            Length = dto.Length,
            Width = dto.Width,
            Height = dto.Height,
            DimensionUnit = dto.DimensionUnit,
            Volume = dto.Volume,
            VolumeUnit = dto.VolumeUnit,
            MinTemperature = dto.MinTemperature,
            MaxTemperature = dto.MaxTemperature,
            TemperatureUnit = dto.TemperatureUnit,

            FreightClass = dto.FreightClass,
            DeclaredValue = dto.DeclaredValue,
            Currency = dto.Currency,
            Stackable = dto.Stackable,
            CopyToLoad = dto.CopyToLoad,
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

        return order.Items
            .OrderBy(i => i.CreatedAt)
            .Select((i, index) => new OrderItemDto
        {
            Id = i.Id,
            Name = i.Name,
            CustomerReference = i.CustomerReference,
            IdentificationNumber = i.IdentificationNumber,
            Quantity = i.Quantity,
            ActualQuantity = i.HandlingQuantity ?? i.Quantity,
            Status = "Active",
            LineItemNumber = index + 1,
            QuantityUnit = i.QuantityUnit,
            IsHazmat = i.IsHazmat,
            FreightClass = i.FreightClass,
            HazardClass = i.HazardClass,
            HandlingQuantity = i.HandlingQuantity,
            HandlingUnit = i.HandlingUnit,
            UnitNetWeight = i.UnitNetWeight,
            UnitGrossWeight = i.UnitGrossWeight,
            WeightUnit = i.WeightUnit,
            Length = i.Length,
            Width = i.Width,
            Height = i.Height,
            DimensionUnit = i.DimensionUnit,
            Volume = i.Volume,
            VolumeUnit = i.VolumeUnit,
            MinTemperature = i.MinTemperature,
            MaxTemperature = i.MaxTemperature,
            TemperatureUnit = i.TemperatureUnit,
            DeclaredValue = i.DeclaredValue,
            Currency = i.Currency,
            Stackable = i.Stackable,
            CopyToLoad = i.CopyToLoad,
            Notes = i.Notes
        }).ToList();
    }

    public async Task UpdateAsync(Guid orderId, Guid itemId, UpdateOrderItemDto dto, Guid userId)
    {
        var order = await _orders.GetByIdAsync(orderId)
            ?? throw new NotFoundException("Order not found.");

        if (!CanEditItems(order.Status))
            throw new BusinessRuleException("Items cannot be changed in this order status.");

        var item = await _items.GetByIdAsync(itemId);
        if (item == null || item.OrderId != orderId)
            throw new NotFoundException("Order item not found.");

        item.Name = dto.Name ?? item.Name;
        item.CustomerReference = dto.CustomerReference ?? item.CustomerReference;
        item.Quantity = dto.Quantity ?? item.Quantity;
        item.QuantityUnit = dto.QuantityUnit ?? item.QuantityUnit;
        item.IsHazmat = dto.IsHazmat ?? item.IsHazmat;
        item.FreightClass = dto.FreightClass ?? item.FreightClass;
        item.HazardClass = dto.HazardClass ?? item.HazardClass;
        item.IdentificationNumber = dto.IdentificationNumber ?? item.IdentificationNumber;
        item.HandlingQuantity = dto.HandlingQuantity ?? dto.ActualQuantity ?? item.HandlingQuantity;
        item.HandlingUnit = dto.HandlingUnit ?? item.HandlingUnit;
        item.UnitNetWeight = dto.UnitNetWeight ?? item.UnitNetWeight;
        item.UnitGrossWeight = dto.UnitGrossWeight ?? item.UnitGrossWeight;
        item.WeightUnit = dto.WeightUnit ?? item.WeightUnit;
        item.Length = dto.Length ?? item.Length;
        item.Width = dto.Width ?? item.Width;
        item.Height = dto.Height ?? item.Height;
        item.DimensionUnit = dto.DimensionUnit ?? item.DimensionUnit;
        item.Volume = dto.Volume ?? item.Volume;
        item.VolumeUnit = dto.VolumeUnit ?? item.VolumeUnit;
        item.MinTemperature = dto.MinTemperature ?? item.MinTemperature;
        item.MaxTemperature = dto.MaxTemperature ?? item.MaxTemperature;
        item.TemperatureUnit = dto.TemperatureUnit ?? item.TemperatureUnit;
        item.DeclaredValue = dto.DeclaredValue ?? item.DeclaredValue;
        item.Currency = dto.Currency ?? item.Currency;
        item.Stackable = dto.Stackable ?? item.Stackable;
        item.CopyToLoad = dto.CopyToLoad ?? item.CopyToLoad;
        item.Notes = dto.Notes ?? item.Notes;

        _items.Update(item);
        await _orders.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid orderId, Guid itemId, Guid userId)
    {
        var order = await _orders.GetByIdAsync(orderId)
            ?? throw new NotFoundException("Order not found.");

        if (!CanEditItems(order.Status))
            throw new BusinessRuleException("Items cannot be changed in this order status.");

        var item = await _items.GetByIdAsync(itemId);
        if (item == null || item.OrderId != orderId)
            throw new NotFoundException("Order item not found.");

        _items.Remove(item);
        await _orders.SaveChangesAsync();
    }

    private static bool CanEditItems(OrderStatus status)
    {
        return status != OrderStatus.Completed
            && status != OrderStatus.Cancelled
            && status != OrderStatus.Billed;
    }
}
