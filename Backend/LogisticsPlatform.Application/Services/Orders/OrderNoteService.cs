using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Orders.Notes;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;
using SendGrid.Helpers.Errors.Model;

namespace LogisticsPlatform.Application.Services.Orders
{
    public class OrderNoteService : IOrderNoteService
    {
        private readonly IOrderNoteRepository _notes;
        private readonly IOrderRepository _orders;
        private readonly IUserRepository _users;
        private readonly IPermissionService _permission;

        public OrderNoteService(
            IOrderNoteRepository notes,
            IOrderRepository orders,
            IUserRepository users,
            IPermissionService permission)
        {
            _notes = notes;
            _orders = orders;
            _users = users;
            _permission = permission;
        }

        public async Task<IReadOnlyList<OrderNoteDto>> GetByOrderAsync(Guid orderId, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found");

            var order = await _orders.GetByIdAsync(orderId)
                ?? throw new NotFoundException("Order not found");

            var notes = (await _notes.GetByOrderAsync(orderId)).ToList();

            if (!await _permission.HasPermissionAsync(userId, Permission.LoadNote_View))
            {
                notes = notes.Where(n => !n.IsInternal).ToList();
            }

            return notes.Select(Map).ToList();
        }

        public async Task<OrderNoteDto> CreateAsync(Guid orderId, CreateOrderNoteDto dto, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found");

            var order = await _orders.GetByIdAsync(orderId)
                ?? throw new NotFoundException("Order not found");

            var neededPermission = dto.IsInternal
                ? Permission.LoadNote_Create_Internal
                : Permission.LoadNote_Create_Public;

            if (!await _permission.HasPermissionAsync(userId, neededPermission))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to add this note.");

            var note = new OrderNote
            {
                OrderId = orderId,
                Message = dto.Message,
                IsInternal = dto.IsInternal,
                CreatedByUserId = userId
            };

            await _notes.AddAsync(note);
            await _orders.SaveChangesAsync();

            var saved = await _notes.GetByIdAsync(note.Id) ?? note;
            return Map(saved);
        }

        public async Task<OrderNoteDto?> UpdateAsync(Guid id, UpdateOrderNoteDto dto, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found");

            if (!await _permission.HasPermissionAsync(userId, Permission.LoadNote_Create_Internal))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to update notes.");

            var note = await _notes.GetByIdAsync(id);
            if (note == null) return null;

            note.Message = dto.Message ?? note.Message;
            note.IsInternal = dto.IsInternal ?? note.IsInternal;

            _notes.Update(note);
            await _orders.SaveChangesAsync();

            var saved = await _notes.GetByIdAsync(note.Id) ?? note;
            return Map(saved);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found");

            if (!await _permission.HasPermissionAsync(userId, Permission.LoadNote_Create_Internal))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to delete notes.");

            var note = await _notes.GetByIdAsync(id);
            if (note == null) return false;

            _notes.Remove(note);
            await _orders.SaveChangesAsync();
            return true;
        }

        private static OrderNoteDto Map(OrderNote n) => new()
        {
            Id = n.Id,
            OrderId = n.OrderId,
            Message = n.Message,
            IsInternal = n.IsInternal,
            CreatedByUserId = n.CreatedByUserId,
            CreatedByName = n.CreatedByUser?.FullName ?? string.Empty,
            CreatedAt = n.CreatedAt
        };
    }
}
