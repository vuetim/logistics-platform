using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using SendGrid.Helpers.Errors.Model;
using System.ComponentModel.DataAnnotations;

namespace LogisticsPlatform.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orders;
        private readonly ICustomerRepository _customers;
        private readonly ICarrierRepository _carriers;
        private readonly IUserRepository _users;

        public OrderService(
            IOrderRepository orders,
            ICustomerRepository customers,
            ICarrierRepository carriers,
            IUserRepository users)
        {
            _orders = orders;
            _customers = customers;
            _carriers = carriers;
            _users = users;
        }

        // CREATE
        public async Task<Guid> CreateAsync(CreateOrderDto dto, Guid userId)
        {
            var user = await GetUserOrThrow(userId);

            var customer = await _customers.GetByIdAsync(dto.CustomerId)
                ?? throw new NotFoundException("Customer not found.");

            Carrier? preferredCarrier = null;
            if (dto.PreferredCarrierId.HasValue)
            {
                preferredCarrier = await _carriers.GetByIdAsync(dto.PreferredCarrierId.Value)
                    ?? throw new NotFoundException("Preferred carrier not found.");
            }

            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),

                CustomerId = customer.Id,
                PreferredCarrierId = preferredCarrier?.Id,

                OrderType = dto.OrderType,
                Direction = dto.Direction,
                Status = OrderStatus.Draft,
                Phase = OrderPhase.Open,

                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                PlannedPickupDate = dto.PlannedPickupDate,
                PlannedDeliveryDate = dto.PlannedDeliveryDate,


                CreatedByUserId = user.Id
            };

            // Child aggregates
            //if (dto.Items != null)
            //    foreach (var item in dto.Items)
            //        order.Items.Add(item.ToEntity());

            //if (dto.EquipmentRequirements != null)
            //    foreach (var eq in dto.EquipmentRequirements)
            //        order.EquipmentRequirements.Add(eq.ToEntity());

            //if (dto.ExternalIds != null)
            //    foreach (var ext in dto.ExternalIds)
            //        order.ExternalIds.Add(ext.ToEntity());

            //if (dto.Notes != null)
            //    foreach (var note in dto.Notes)
            //        order.Notes.Add(note.ToEntity(user.Id));

            //if (dto.Documents != null)
            //    foreach (var doc in dto.Documents)
            //        order.Documents.Add(doc.ToEntity());

            //if (dto.Routes != null)
            //    foreach (var route in dto.Routes)
            //        order.OrderRoutes.Add(route.ToEntity());

            //if (dto.Cost != null)
            //    order.Cost = dto.Cost.ToEntity();

            await _orders.AddAsync(order);
            await _orders.SaveChangesAsync();

            return order.Id;
        }

        // UPDATE
        public async Task UpdateAsync(Guid id, UpdateOrderDto dto, Guid userId)
        {
            var order = await _orders.GetByIdAsync(id)
                ?? throw new NotFoundException("Order not found.");

            // basic guards
            if (order.Status == OrderStatus.Completed)
                throw new ValidationException("Completed order cannot be modified.");

            //order.StartDate = dto.StartDate ?? order.StartDate;
            //order.EndDate = dto.EndDate ?? order.EndDate;
            order.PlannedPickupDate = dto.PlannedPickupDate ?? order.PlannedPickupDate;
            order.PlannedDeliveryDate = dto.PlannedDeliveryDate ?? order.PlannedDeliveryDate;

            // child updates (simple strategy: replace collections)
            //if (dto.Items != null)
            //{
            //    order.Items.Clear();
            //    foreach (var item in dto.Items)
            //        order.Items.Add(item.ToEntity());
            //}

            //if (dto.EquipmentRequirements != null)
            //{
            //    order.EquipmentRequirements.Clear();
            //    foreach (var eq in dto.EquipmentRequirements)
            //        order.EquipmentRequirements.Add(eq.ToEntity());
            //}

            //if (dto.Routes != null)
            //{
            //    order.OrderRoutes.Clear();
            //    foreach (var route in dto.Routes)
            //        order.OrderRoutes.Add(route.ToEntity());
            //}

            //if (dto.Cost != null)
            //    order.Cost = dto.Cost.ToEntity();

            await _orders.UpdateAsync(order);
            await _orders.SaveChangesAsync();
        }

        // CHANGE STATUS
        public async Task ChangeStatusAsync(Guid id, OrderStatus newStatus, Guid userId)
        {
            var order = await _orders.GetByIdAsync(id)
                ?? throw new NotFoundException("Order not found.");

            // basic state guards (state machine do ta bëjmë më vonë)
            if (order.Status == OrderStatus.Completed)
                throw new ValidationException("Order already completed.");

            if (order.Status == OrderStatus.Cancelled)
                throw new ValidationException("Cancelled order cannot change status.");

            order.Status = newStatus;

            // Phase sync (high-level)
            order.Phase = newStatus switch
            {
                OrderStatus.Draft or OrderStatus.Submitted => OrderPhase.Open,
                OrderStatus.Scheduled or OrderStatus.Dispatched => OrderPhase.Ship,
                OrderStatus.ReadyForBilling or OrderStatus.Billed => OrderPhase.Bill,
                OrderStatus.Completed => OrderPhase.Complete,
                OrderStatus.Cancelled => OrderPhase.Cancelled,
                _ => order.Phase
            };

            await _orders.UpdateAsync(order);
            await _orders.SaveChangesAsync();
        }

        // ===== helpers =====

        private static string GenerateOrderNumber()
            => $"O-{DateTime.UtcNow:yyyyMMddHHmmss}";

        private async Task<User> GetUserOrThrow(Guid userId)
        {
            return await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found.");
        }
    }
}
