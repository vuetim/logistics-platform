using LogisticsPlatform.Application.DTOs.ActivityLog;
using LogisticsPlatform.Application.DTOs.Costs;
using LogisticsPlatform.Application.Extensions;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Services.ActivityLog;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
using SendGrid.Helpers.Errors.Model;
using System;
using ForbiddenException = LogisticsPlatform.Application.Common.Exceptions.ForbiddenException;

namespace LogisticsPlatform.Application.Services.Orders;

public class OrderCostService : IOrderCostService
{
    private readonly IOrderRepository _orders;
    private readonly IOrderCostRepository _costs;
    private readonly IActivityLogService _activityLog;
    private readonly IPermissionService _permission;

    public OrderCostService(
        IOrderRepository orders,
        IOrderCostRepository costs,
        IActivityLogService activityLog,
        IPermissionService permission)
    {
        _orders = orders;
        _costs = costs;
        _activityLog = activityLog;
        _permission = permission;
    }

    public async Task<OrderCostDto> GetAsync(Guid orderId, Guid userId)
    {
        if (!await _permission.HasPermissionAsync(userId, Permission.OrderCost_View))
            throw new ForbiddenException("You are not allowed to view order costs.");

        var order = await _orders.GetByIdAsync(orderId)
            ?? throw new NotFoundException("Order not found.");

        var cost = await _costs.GetByOrderIdAsync(orderId);
        if (cost == null)
        {
            return new OrderCostDto
            {
                BillTo = null,
                Notes = null,
                TaxRate = 0,
                BaseFreight = 0,
                Accessorials = 0,
                QuotedTotal = 0,
                SubTotal = 0,
                TotalTax = 0,
                TotalAmount = 0,
                TotalBillable = 0,
                TotalNonBillable = 0,
                LineItems = new()
            };
        }

        var baseFreight = cost.LineItems
            .Where(li => li.IsCustomer && li.Type == ChargeType.Linehaul)
            .Sum(li => li.Amount);
        var totalBillable = cost.LineItems.Where(li => li.IsCustomer).Sum(li => li.Amount);
        var totalNonBillable = cost.LineItems.Where(li => !li.IsCustomer).Sum(li => li.Amount);
        var accessorials = totalBillable - baseFreight;
        var taxRate = cost.TaxRate < 0 ? 0 : cost.TaxRate > 100 ? 100 : cost.TaxRate;
        var totalTax = decimal.Round(totalBillable * taxRate / 100m, 2, MidpointRounding.AwayFromZero);


        return new OrderCostDto
        {
            BillTo = cost.BillTo,
            Notes = cost.Notes,
            TaxRate = taxRate,
            BaseFreight = baseFreight,
            Accessorials = accessorials,
            QuotedTotal = totalBillable + totalTax,
            SubTotal = totalBillable,
            TotalTax = totalTax,
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
        if (!await _permission.HasPermissionAsync(userId, Permission.OrderCost_Update))
            throw new ForbiddenException("You are not allowed to update order costs.");

        var order = await _orders.GetByIdWithLoadsAsync(orderId)
            ?? throw new NotFoundException("Order not found.");

        dto ??= new UpdateOrderCostDto();

        var cost = await _costs.GetByOrderIdForUpdateAsync(orderId);
        if (cost == null)
        {
            cost = new OrderCost
            {
                OrderId = orderId,
                BillTo = dto.BillTo,
                Notes = dto.Notes,
                LineItems = new List<OrderCostLineItem>()
            };

            await _costs.AddAsync(cost);
        }

        cost.BillTo = dto.BillTo;
        cost.Notes = dto.Notes;
        cost.TaxRate = dto.TaxRate < 0 ? 0 : dto.TaxRate > 100 ? 100 : dto.TaxRate;
        await _costs.DeleteLineItemsByOrderCostIdAsync(cost.Id);

        var newLineItems = new List<OrderCostLineItem>();

        foreach (var liDto in dto.LineItems ?? new List<CostLineItemDto>())
        {
            var qty = liDto.Qty < 0 ? 0 : liDto.Qty;
            var price = liDto.Price < 0 ? 0 : liDto.Price;
            var amount = qty * price;
            var type = Enum.IsDefined(typeof(ChargeType), liDto.Type)
                ? liDto.Type
                : ChargeType.Other;

            newLineItems.Add(new OrderCostLineItem
            {
                OrderCostId = cost.Id,
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

        var totalBillable = newLineItems.Where(x => x.IsCustomer).Sum(x => x.Amount);
        var totalNonBillable = newLineItems.Where(x => !x.IsCustomer).Sum(x => x.Amount);
        var baseFreight = newLineItems
            .Where(x => x.IsCustomer && x.Type == ChargeType.Linehaul)
            .Sum(x => x.Amount);
        var totalTax = decimal.Round(totalBillable * cost.TaxRate / 100m, 2, MidpointRounding.AwayFromZero);

        cost.TotalAmount = totalBillable + totalNonBillable + totalTax;

        // sync base freight rate (linehaul only) to order
        order.SetCustomerRate(baseFreight);

        await _orders.SaveChangesAsync();

        try
        {
            await _activityLog.LogAsync(new ActivityLogEntry
            {
                EntityType = "Order",
                EntityId = orderId,
                ActivityType = ActivityType.Order_CostUpdated,
                Summary = $"Order cost updated: BaseFreight={baseFreight}, Billable={totalBillable}, Tax={totalTax}, NonBillable={totalNonBillable}",
                PerformedByUserId = userId
            });
        }
        catch
        {
            // Cost update is primary; activity log failure should not fail request
        }
    }
}
