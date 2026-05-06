using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Orders.Documents;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;
using SendGrid.Helpers.Errors.Model;

namespace LogisticsPlatform.Application.Services.Orders
{
    public class OrderDocumentService : IOrderDocumentService
    {
        private readonly IOrderDocumentRepository _documents;
        private readonly IOrderRepository _orders;
        private readonly IUserRepository _users;
        private readonly IPermissionService _permission;

        public OrderDocumentService(
            IOrderDocumentRepository documents,
            IOrderRepository orders,
            IUserRepository users,
            IPermissionService permission)
        {
            _documents = documents;
            _orders = orders;
            _users = users;
            _permission = permission;
        }

        public async Task<IReadOnlyList<OrderDocumentDto>> GetByOrderAsync(Guid orderId, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found");

            var order = await _orders.GetByIdAsync(orderId)
                ?? throw new NotFoundException("Order not found");

            var docs = (await _documents.GetByOrderAsync(orderId)).ToList();

            if (!await _permission.HasPermissionAsync(userId, Permission.LoadDocument_View))
            {
                docs = docs.Where(d => !d.IsInternal).ToList();
            }

            return docs.Select(Map).ToList();
        }

        public async Task<OrderDocumentDto> CreateAsync(Guid orderId, CreateOrderDocumentDto dto, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found");

            var order = await _orders.GetByIdAsync(orderId)
                ?? throw new NotFoundException("Order not found");

            if (!await _permission.HasPermissionAsync(userId, Permission.LoadDocument_Upload))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to upload order documents.");

            var doc = new OrderDocument
            {
                OrderId = orderId,
                DocumentType = dto.DocumentType,
                FileUrl = dto.FileUrl,
                IsInternal = dto.IsInternal,
                CopyToLoad = dto.CopyToLoad
            };

            await _documents.AddAsync(doc);
            await _documents.SaveChangesAsync();

            var saved = await _documents.GetByIdAsync(doc.Id) ?? doc;
            return Map(saved);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found");

            if (!await _permission.HasPermissionAsync(userId, Permission.LoadDocument_Delete))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to delete order documents.");

            var doc = await _documents.GetByIdAsync(id);
            if (doc == null) return false;

            _documents.Remove(doc);
            await _documents.SaveChangesAsync();
            return true;
        }

        private static OrderDocumentDto Map(OrderDocument d) => new()
        {
            Id = d.Id,
            DocumentType = d.DocumentType,
            FileUrl = d.FileUrl,
            IsInternal = d.IsInternal,
            CopyToLoad = d.CopyToLoad,
            CreatedAt = d.CreatedAt
        };
    }
}
