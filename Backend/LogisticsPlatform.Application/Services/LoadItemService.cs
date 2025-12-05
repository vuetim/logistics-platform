using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.ActivityLog;
using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using SendGrid.Helpers.Errors.Model;

public class LoadItemService : ILoadItemService
{
    private readonly ILoadItemRepository _items;
    private readonly ILoadRepository _loads;
    private readonly IActivityLogService _activityLog;


    public LoadItemService(
        ILoadItemRepository items,
        ILoadRepository loads, IActivityLogService activityLog)
    {
        _items = items;
        _loads = loads;
        _activityLog = activityLog;
    }

    public async Task UpdateAsync(
        Guid loadId,
        Guid itemId,
        UpdateLoadItemDto dto,
        Guid userId)
    {
        var load = await _loads.GetByIdAsync(loadId)
            ?? throw new NotFoundException("Load not found.");

        // ❌ locked loads
        if (load.Status >= LoadStatus.Completed)
            throw new BusinessRuleException("Cannot edit items on completed load.");

        var item = await _items.GetByIdAsync(loadId, itemId)
            ?? throw new NotFoundException("Load item not found.");

        // ✅ allowed partial update
        item.HandlingQuantity = dto.HandlingQuantity ?? item.HandlingQuantity;
        item.HandlingUnit = dto.HandlingUnit ?? item.HandlingUnit;

        item.UnitNetWeight = dto.UnitNetWeight ?? item.UnitNetWeight;
        item.UnitGrossWeight = dto.UnitGrossWeight ?? item.UnitGrossWeight;
        item.WeightUnit = dto.WeightUnit ?? item.WeightUnit;

        item.Length = dto.Length ?? item.Length;
        item.Width = dto.Width ?? item.Width;
        item.Height = dto.Height ?? item.Height;
        item.DimensionUnit = dto.DimensionUnit ?? item.DimensionUnit;

        item.MinTemperature = dto.MinTemperature ?? item.MinTemperature;
        item.MaxTemperature = dto.MaxTemperature ?? item.MaxTemperature;
        item.TemperatureUnit = dto.TemperatureUnit ?? item.TemperatureUnit;

        item.DeclaredValue = dto.DeclaredValue ?? item.DeclaredValue;
        item.Currency = dto.Currency ?? item.Currency;

        if (dto.Stackable.HasValue)
            item.Stackable = dto.Stackable.Value;

        item.Notes = dto.Notes ?? item.Notes;

        await _items.SaveChangesAsync();
        await _activityLog.LogAsync(new ActivityLogEntry
        {
            EntityType = ActivityEntityType.Load.ToString(),
            EntityId = loadId,
            ActivityType = ActivityType.Load_ItemUpdated,
            PerformedByUserId = userId,
            Summary = $"Load item '{item.Name}' updated",
            Details = null
        });
    }

}
