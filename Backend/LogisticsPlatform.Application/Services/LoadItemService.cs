using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.ActivityLog;
using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Services.ActivityLog;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Enums;
using SendGrid.Helpers.Errors.Model;
using System.Diagnostics;

public class LoadItemService : ILoadItemService
{
    private readonly ILoadItemRepository _items;
    private readonly ILoadRepository _loads;
    private readonly IActivityLogService _activityLog;
    private readonly IOrderRepository _orders;



    public LoadItemService(
        ILoadItemRepository items,
        ILoadRepository loads, IActivityLogService activityLog, IOrderRepository orders )
    {
        _items = items;
        _loads = loads;
        _activityLog = activityLog;
        _orders = orders;
        
    }


    public async Task AddFromOrderItemAsync(Guid loadId, Guid orderId, Guid orderItemId, Guid userId)
    {
        var load = await _loads.GetByIdAsync(loadId)
            ?? throw new NotFoundException("Load not found.");

        var order = await _orders.GetByIdWithItemsAsync(orderId)
            ?? throw new NotFoundException("Order not found.");

        var orderItem = order.Items
            .FirstOrDefault(i => i.Id == orderItemId)
            ?? throw new NotFoundException("Order item not found.");

        var item = new LoadItem
        {
            LoadId = loadId,
            SourceOrderItemId = orderItem.Id,

            Name = orderItem.Name,
            Quantity = orderItem.Quantity,
            QuantityUnit = orderItem.QuantityUnit,
            IsHazmat = orderItem.IsHazmat,
            FreightClass = orderItem.FreightClass,
            Notes = orderItem.Notes
        };

        await _items.AddAsync(item);
        await _items.SaveChangesAsync();

        await _activityLog.LogAsync(new ActivityLogEntry
        {
            EntityType = "Load",
            EntityId = loadId,
            ActivityType = ActivityType.Load_ItemCreated,
            Summary = $"Load item '{item.Name}' added from order item {orderItem.Id}.",
            Details = null,
            PerformedByUserId = userId
        });
    }


    // UPDATE
    public async Task UpdateAsync(Guid loadId, Guid itemId, UpdateLoadItemDto dto, Guid userId)
    {
        var item = await _items.GetByIdAsync(loadId, itemId)
            ?? throw new NotFoundException("Item not found.");

        if (dto.HandlingQuantity.HasValue) item.HandlingQuantity = dto.HandlingQuantity;
        if (dto.HandlingUnit != null) item.HandlingUnit = dto.HandlingUnit;

        if (dto.UnitNetWeight.HasValue) item.UnitNetWeight = dto.UnitNetWeight;
        if (dto.UnitGrossWeight.HasValue) item.UnitGrossWeight = dto.UnitGrossWeight;
        if (dto.WeightUnit != null) item.WeightUnit = dto.WeightUnit;

        if (dto.Length.HasValue) item.Length = dto.Length;
        if (dto.Width.HasValue) item.Width = dto.Width;
        if (dto.Height.HasValue) item.Height = dto.Height;
        if (dto.DimensionUnit != null) item.DimensionUnit = dto.DimensionUnit;

        //if (dto.Volume.HasValue) item.Volume = dto.Volume;
        //if (dto.VolumeUnit != null) item.VolumeUnit = dto.VolumeUnit;

        if (dto.MinTemperature.HasValue) item.MinTemperature = dto.MinTemperature;
        if (dto.MaxTemperature.HasValue) item.MaxTemperature = dto.MaxTemperature;
        if (dto.TemperatureUnit != null) item.TemperatureUnit = dto.TemperatureUnit;

        if (dto.Stackable.HasValue) item.Stackable = dto.Stackable.Value;
        if (dto.DeclaredValue.HasValue) item.DeclaredValue = dto.DeclaredValue;
        if (dto.Currency != null) item.Currency = dto.Currency;

        if (dto.Notes != null) item.Notes = dto.Notes;

        await _items.SaveChangesAsync();

        await _activityLog.LogAsync(new ActivityLogEntry
        {
            EntityType = "Load",
            EntityId = loadId,
            ActivityType = ActivityType.Load_ItemUpdated,
            PerformedByUserId = userId,
            Summary = $"Load item '{item.Name}' updated.",
            Details = null
        });
    }
    // DELETE
    public async Task DeleteAsync(Guid loadId, Guid itemId,  Guid userId)
    {
        var item = await _items.GetByIdAsync(loadId, itemId)
            ?? throw new NotFoundException("Item not found.");

        await _items.DeleteAsync(item);
        await _items.SaveChangesAsync();

        await _activityLog.LogAsync(new ActivityLogEntry
        {
            EntityType = "Load",
            EntityId = loadId,
            ActivityType = ActivityType.Load_ItemDeleted,
            PerformedByUserId = userId,
            Summary = $"Load item '{item.Name}' deleted.",
            Details = null
        });
    }

}
