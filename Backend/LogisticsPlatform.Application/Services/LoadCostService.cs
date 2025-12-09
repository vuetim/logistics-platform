using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.ActivityLog;
using LogisticsPlatform.Application.DTOs.Costs;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using SendGrid.Helpers.Errors.Model;
using LogisticsPlatform.Application.Extensions;

using System.Collections.Generic;

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

        var cost = await _costs.GetByLoadIdAsync(loadId);

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

        // replace all
        cost.Notes = dto.Notes;
        cost.LineItems.Clear();

        foreach (var liDto in dto.LineItems)
        {
            var amount = liDto.Qty * liDto.Price;

            cost.LineItems.Add(new LoadCostLineItem
            {
                Type = liDto.Type,
                Qty = liDto.Qty,
                Price = liDto.Price,
                Amount = amount,
                IsCustomer = liDto.IsCustomer,
                IsCarrier = liDto.IsCarrier,
                Notes = liDto.Notes
            });
        }

        // totals
        var totalCarrier = cost.LineItems
            .Where(x => x.IsCarrier)
            .Sum(x => x.Amount);

        var totalCustomer = cost.LineItems
            .Where(x => x.IsCustomer)
            .Sum(x => x.Amount);

        // sync to load
        cost.TotalAmount = totalCarrier;

        load.CarrierRate = totalCarrier;

        // optional passthrough logic
        if (!load.Orders.Any())
        {
            load.CustomerRate = totalCustomer;
        }

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
