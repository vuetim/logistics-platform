using LogisticsPlatform.Application.DTOs.ActivityLog;
using LogisticsPlatform.Application.DTOs.Costs;
using LogisticsPlatform.Application.Extensions;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Services.ActivityLog;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using SendGrid.Helpers.Errors.Model;
using System;

namespace LogisticsPlatform.Application.Services.Orders;

public class OrderCostService : IOrderCostService
{
    private readonly IOrderRepository _orders;
    private readonly IOrderCostRepository _costs;
    private readonly IActivityLogService _activityLog;

    public OrderCostService(
        IOrderRepository orders,
        IOrderCostRepository costs,
        IActivityLogService activityLog)
    {
        _orders = orders;
        _costs = costs;
        _activityLog = activityLog;
    }

    public async Task<OrderCostDto> GetAsync(Guid orderId)
    {
        var order = await _orders.GetByIdAsync(orderId)
            ?? throw new NotFoundException("Order not found.");

        var cost = await _costs.GetByOrderIdAsync(orderId);
        if (cost == null)
        {
            return new OrderCostDto
            {
                Notes = null,
                TotalAmount = 0,
                TotalBillable = 0,
                TotalNonBillable = 0,
                LineItems = new()
            };
        }

        var totalBillable = cost.LineItems.Where(li => li.IsCustomer).Sum(li => li.Amount);
        var totalNonBillable = cost.LineItems.Where(li => !li.IsCustomer).Sum(li => li.Amount);


        return new OrderCostDto
        {
            Notes = cost.Notes,
            TotalAmount = cost.TotalAmount,
            TotalBillable = totalBillable,
            TotalNonBillable = totalNonBillable,
            LineItems = cost.LineItems.Select(li => new CostLineItemDto
            {
                Id = li.Id,
             Type = li.Type,
                Qty = li.Qty,
                Price = li.Price,
                IsCustomer = li.IsCustomer,
                Notes = li.Notes
            }).ToList()
        };
    }

    public async Task UpdateAsync(Guid orderId, UpdateOrderCostDto dto, Guid userId)
    {
        var order = await _orders.GetByIdWithLoadsAsync(orderId)
            ?? throw new NotFoundException("Order not found.");

        var cost = await _costs.GetByOrderIdAsync(orderId);
        if (cost == null)
        {
            cost = new OrderCost
            {
                OrderId = orderId,
                Notes = dto.Notes,
                LineItems = new List<OrderCostLineItem>()
            };

            await _costs.AddAsync(cost);
        }

        cost.Notes = dto.Notes;
        cost.LineItems.Clear();

        foreach (var liDto in dto.LineItems)
        {
            var amount = liDto.Qty * liDto.Price;

            cost.LineItems.Add(new OrderCostLineItem
            {
                Type = liDto.Type,
                Qty = liDto.Qty,
                Price = liDto.Price,
                Amount = amount,
                IsCustomer = liDto.IsCustomer,
                IsCarrier = false,
                Notes = liDto.Notes
            });
        }

        var totalBillable = cost.LineItems.Where(x => x.IsCustomer).Sum(x => x.Amount);
        var totalNonBillable = cost.LineItems.Where(x => !x.IsCustomer).Sum(x => x.Amount);

        cost.TotalAmount = totalBillable + totalNonBillable;

        // sync to order
        order.CustomerRate = totalBillable;

        // sync all loads
        foreach (var link in order.Loads)
        {
            if (link.Load is not null)
            {
                link.Load.CustomerRate = totalBillable;
                link.Load.RecalculateFinancials();

            }
        }

        await _orders.SaveChangesAsync();

        await _activityLog.LogAsync(new ActivityLogEntry
        {
            EntityType = "Order",
            EntityId = orderId,
            ActivityType = ActivityType.Order_CostUpdated,
            Summary = $"Order cost updated: Billable={totalBillable}, NonBillable={totalNonBillable}",
            PerformedByUserId = userId
        });
    }
}
