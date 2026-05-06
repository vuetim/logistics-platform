using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Orders.ExternalIds;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.Services.Orders
{
    public class OrderExternalIdService : IOrderExternalIdService
    {
        private readonly IOrderExternalIdRepository _externalIds;
        private readonly IOrderRepository _orders;
        private readonly IUserRepository _users;
        private readonly IPermissionService _permission;

        public OrderExternalIdService(
            IOrderExternalIdRepository externalIds,
            IOrderRepository orders,
            IUserRepository users,
            IPermissionService permission)
        {
            _externalIds = externalIds;
            _orders = orders;
            _users = users;
            _permission = permission;
        }

        public async Task<IReadOnlyList<OrderExternalIdDto>> GetByOrderAsync(Guid orderId, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_View))
                throw new ForbiddenException("You are not allowed to view external IDs.");

            var order = await _orders.GetByIdAsync(orderId)
                ?? throw new SendGrid.Helpers.Errors.Model.NotFoundException("Order not found");

            var ids = await _externalIds.GetByOrderAsync(orderId);
            return ids.Select(Map).ToList();
        }

        public async Task<OrderExternalIdDto> CreateAsync(Guid orderId, CreateOrderExternalIdDto dto, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Update))
                throw new ForbiddenException("You are not allowed to create external IDs.");

            var order = await _orders.GetByIdAsync(orderId)
                ?? throw new SendGrid.Helpers.Errors.Model.NotFoundException("Order not found");

            var type = NormalizeType(dto.Type);
            var value = (dto.Value ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessRuleException("Reference value is required.");

            var ext = new OrderExternalId
            {
                OrderId = orderId,
                Type = type,
                Value = value,
                RelatedParty = NormalizeRelatedParty(dto.RelatedParty),
                CopyToLoad = dto.CopyToLoad
            };

            await _externalIds.AddAsync(ext);
            await _externalIds.SaveChangesAsync();

            return Map(ext);
        }

        public async Task<OrderExternalIdDto?> UpdateAsync(Guid id, UpdateOrderExternalIdDto dto, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Update))
                throw new ForbiddenException("You are not allowed to update external IDs.");

            var ext = await _externalIds.GetByIdAsync(id);
            if (ext == null) return null;

            if (dto.Type is not null)
                ext.Type = NormalizeType(dto.Type);

            if (dto.Value is not null)
            {
                var value = dto.Value.Trim();
                if (string.IsNullOrWhiteSpace(value))
                    throw new BusinessRuleException("Reference value is required.");
                ext.Value = value;
            }

            if (dto.RelatedParty is not null)
            {
                ext.RelatedParty = NormalizeRelatedParty(dto.RelatedParty);
            }

            ext.CopyToLoad = dto.CopyToLoad ?? ext.CopyToLoad;

            _externalIds.Update(ext);
            await _externalIds.SaveChangesAsync();
            return Map(ext);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Update))
                throw new ForbiddenException("You are not allowed to delete external IDs.");

            var ext = await _externalIds.GetByIdAsync(id);
            if (ext == null) return false;

            _externalIds.Remove(ext);
            await _externalIds.SaveChangesAsync();
            return true;
        }

        private static OrderExternalIdDto Map(OrderExternalId e) => new()
        {
            Id = e.Id,
            OrderId = e.OrderId,
            Type = (e.Type ?? string.Empty).Trim(),
            Value = e.Value,
            RelatedParty = e.RelatedParty,
            CopyToLoad = e.CopyToLoad,
            CreatedAt = e.CreatedAt
        };

        private static string NormalizeType(string? rawType)
        {
            var type = (rawType ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(type))
                throw new BusinessRuleException("Reference type is required.");

            return type switch
            {
                "Po" => "PO",
                "Bol" => "BOL",
                "Pro" => "PRO",
                "PurchaseOrder" => "PO",
                "BillOfLading" => "BOL",
                _ => type
            };
        }

        private static string NormalizeRelatedParty(string? rawRelatedParty)
        {
            var party = (rawRelatedParty ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(party))
                return "Customer";

            return party switch
            {
                "Broker Team" => "BrokerTeam",
                "BrokerTeam" => "BrokerTeam",
                "Customer" => "Customer",
                "Carrier" => "Carrier",
                "Warehouse" => "Warehouse",
                "Buyer" => "Buyer",
                "Other" => "Other",
                _ => "Other"
            };
        }
    }
}
