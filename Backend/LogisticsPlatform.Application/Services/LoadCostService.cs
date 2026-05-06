using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.ActivityLog;
using LogisticsPlatform.Application.DTOs.Costs;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using SendGrid.Helpers.Errors.Model;
using LogisticsPlatform.Application.Extensions;

using System.Collections.Generic;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.ActivityLog;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;

namespace LogisticsPlatform.Application.Services;

public class LoadCostService : ILoadCostService
{
    private readonly ILoadRepository _loads;
    private readonly ILoadCostRepository _costs;
    private readonly IActivityLogService _activityLog;

    public LoadCostService(
        ILoadRepository loads,
        ILoadCostRepository costs,
        IActivityLogService activityLog)
    {
        _loads = loads;
        _costs = costs;
        _activityLog = activityLog;
    }

    public async Task<LoadCostDto> GetAsync(Guid loadId)
    {
        var load = await _loads.GetByIdAsync(loadId)
            ?? throw new NotFoundException("Load not found.");

        var cost = await _costs.GetByLoadIdAsync(loadId);
        if (cost == null)
        {
            return new LoadCostDto
            {
                Notes = null,
                TotalAmount = 0,
                LineItems = new()
            };
        }

        return new LoadCostDto
        {
            Notes = cost.Notes,
            TotalAmount = cost.TotalAmount,
            LineItems = cost.LineItems.Select(li => new CostLineItemDto
            {
                Id = li.Id,
                Type = li.Type,
                Qty = li.Qty,
                Price = li.Price,
                Amount = li.Amount,
                IsCustomer = li.IsCustomer,
                IsCarrier = li.IsCarrier,
                Notes = li.Notes
            }).ToList()
        };
    }

    public async Task UpdateAsync(Guid loadId, UpdateLoadCostDto dto, Guid userId)
    {
        var load = await _loads.GetByIdAsync(loadId)
            ?? throw new NotFoundException("Load not found.");

        if (load.Status == LoadStatus.Completed)
            throw new BusinessRuleException("Completed load costs cannot be changed.");

        var cost = await _costs.GetByLoadIdForUpdateAsync(loadId);

        if (cost == null)
        {
            cost = new LoadCost
            {
                LoadId = loadId,
                Notes = dto.Notes,
                LineItems = new List<LoadCostLineItem>()
            };

            await _costs.AddAsync(cost);
        }

        cost.Notes = dto.Notes;

        await _costs.DeleteLineItemsByLoadCostIdAsync(cost.Id);
        var newLineItems = new List<LoadCostLineItem>();
        foreach (var liDto in dto.LineItems)
        {
            var qty = liDto.Qty < 0 ? 0 : liDto.Qty;
            var price = liDto.Price < 0 ? 0 : liDto.Price;
            var amount = qty * price;
            var type = Enum.IsDefined(typeof(ChargeType), liDto.Type)
                ? liDto.Type
                : ChargeType.Other;

            newLineItems.Add(new LoadCostLineItem
            {
                LoadCostId = cost.Id,
                Type = type,
                Qty = qty,
                Price = price,
                Amount = amount,
                IsCustomer = liDto.IsCustomer,
                IsCarrier = liDto.IsCarrier,
                Notes = liDto.Notes
            });
        }

        if (newLineItems.Count > 0)
        {
            await _costs.AddLineItemsAsync(newLineItems);
        }

        // totals
        var totalCarrier = newLineItems
            .Where(x => x.IsCarrier)
            .Sum(x => x.Amount);

        var totalCustomer = newLineItems
            .Where(x => x.IsCustomer)
            .Sum(x => x.Amount);

        cost.TotalAmount = totalCarrier;

        // recalc margin
        load.RecalculateFinancials();

        await _loads.SaveChangesAsync();

        await _activityLog.LogAsync(new ActivityLogEntry
        {
            EntityType = "Load",
            EntityId = loadId,
            ActivityType = ActivityType.Load_CostUpdated,
            Summary = $"Load cost updated: Carrier={totalCarrier}, Customer={totalCustomer}",
            PerformedByUserId = userId
        });
    }
}
