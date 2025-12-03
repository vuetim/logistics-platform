using LogisticsPlatform.Application.Authorization;
using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
using SendGrid.Helpers.Errors.Model;

namespace LogisticsPlatform.Application.Services
{
    public class LoadService : ILoadService
    {
        private readonly ILoadRepository _loads;
        private readonly ICustomerRepository _customers;
        private readonly ICarrierRepository _carriers;
        private readonly IUserRepository _users;
        private readonly IAuthorizationService _auth;
        private readonly IOrderRepository _orders;



        public LoadService(
            ILoadRepository loads,
            ICustomerRepository customers,
            ICarrierRepository carriers,
            IUserRepository users,
            IAuthorizationService auth,
            IOrderRepository orders)
        {
            _loads = loads;
            _customers = customers;
            _carriers = carriers;
            _users = users;
            _auth = auth;
            _orders = orders;
        }

        // ✅ CREATE LOAD (Admin, Broker)
        public async Task<Guid> CreateAsync(CreateLoadDto dto, Guid userId)
        {
            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_Create))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to create loads.");

            var customer = await _customers.GetByIdAsync(dto.CustomerId)
                ?? throw new Exception("Customer not found");

            Carrier? carrier = null;
            if (dto.CarrierId.HasValue)
            {
                carrier = await _carriers.GetByIdAsync(dto.CarrierId.Value)
                    ?? throw new Exception("Carrier not found");
            }

            var load = new Load
            {
                LoadNumber = $"L-{DateTime.UtcNow:yyyyMMddHHmmss}",
                CustomerId = customer.Id,
                CarrierId = carrier?.Id,

                Origin = dto.Origin,
                Destination = dto.Destination,
                Mode = dto.ShipmentType,
                Status = LoadStatus.Draft,

                CustomerRate = dto.CustomerRate,
                CarrierRate = dto.CarrierRate,
                Accessorials = dto.Accessorials,

                IsTemperatureControlled = dto.IsTemperatureControlled,
                IsArchived = false,

                CreatedByUserId = user.Id
            };

            await _loads.AddAsync(load);
            await _loads.SaveChangesAsync();

            return load.Id;
        }

        //  UPDATE LOAD
        public async Task UpdateAsync(Guid id, UpdateLoadDto dto, Guid userId)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new Exception("Load not found");

            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_Update, load))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to update this load.");

            load.Origin = dto.Origin ?? load.Origin;
            load.Destination = dto.Destination ?? load.Destination;

            if (dto.ModeType.HasValue)
                load.Mode = dto.ModeType.Value;

            if (dto.CustomerRate.HasValue)
                load.CustomerRate = dto.CustomerRate.Value;

            if (dto.CarrierRate.HasValue)
                load.CarrierRate = dto.CarrierRate.Value;

            load.Accessorials = dto.Accessorials;

            if (dto.CarrierId.HasValue)
                load.CarrierId = dto.CarrierId.Value;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
        }

        //  CHANGE LOAD STATUS
        public async Task ChangeStatusAsync(Guid id, LoadStatus newStatus, Guid userId)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new Exception("Load not found");

            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_ChangeStatus, load))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to change load status.");

            if (load.Status == LoadStatus.Completed)
                throw new Exception("Completed load cannot be changed.");

            load.Status = newStatus;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
        }

        //loadorder

        public async Task<Guid> CreateFromOrderAsync(
        CreateLoadFromOrderDto dto,
        Guid userId)
        {
            // 1️⃣ Order + routes
            var order = await _orders.GetByIdWithRoutesAsync(dto.OrderId)
                ?? throw new NotFoundException("Order not found.");

            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_CreateFromOrder))
                throw new Common.Exceptions.ForbiddenException("Not allowed to create load from order.");

            var routes = order.OrderRoutes
                .Where(r => r.CopyToLoad && r.IsActive)
                .OrderBy(r => r.Sequence)
                .ToList();

            if (!routes.Any())
                throw new BusinessRuleException("No active routes to copy.");

            // 2️⃣ Create Load (ONCE)
            var load = new Load
            {
                LoadNumber = $"L-{DateTime.UtcNow:yyyyMMddHHmmss}",
                CustomerId = order.CustomerId,
                CarrierId = dto.CarrierId ?? order.PreferredCarrierId,

                Status = LoadStatus.Draft,
                Mode = ModeType.TL,

                CustomerRate = dto.CustomerRate ?? 0,
                CarrierRate = dto.CarrierRate ?? 0,

                Origin = $"{routes.First().City}, {routes.First().State}",
                Destination = $"{routes.Last().City}, {routes.Last().State}",

                IsArchived = false,
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _loads.AddAsync(load);
            await _loads.SaveChangesAsync(); // ✅ LoadId exists now

            // 3️⃣ Create LoadStops (snapshot)
            foreach (var route in routes)
            {
                await _loads.AddStopAsync(new LoadStop
                {
                    LoadId = load.Id,

                    Sequence = route.Sequence,
                    StopType = route.StopType,

                    LocationName = route.LocationName,
                    AddressLine1 = route.AddressLine1,
                    AddressLine2 = route.AddressLine2,
                    City = route.City,
                    State = route.State,
                    PostalCode = route.PostalCode,
                    Country = route.Country,
                    Latitude = route.Latitude,
                    Longitude = route.Longitude,

                    PlannedArrivalFrom = route.PlannedArrivalFrom,
                    PlannedArrivalTo = route.PlannedArrivalTo,
                    PlannedDepartureFrom = route.PlannedDepartureFrom,
                    PlannedDepartureTo = route.PlannedDepartureTo,

                    AppointmentType = route.AppointmentType,
                    FlexMinutes = route.FlexMinutes,

                    Status = StopStatus.Pending,
                    Notes = route.Notes
                });
            }

            await _loads.SaveChangesAsync();

            // 4️⃣ Link Order ↔ Load
            await _loads.AddLoadOrderAsync(new LoadOrder
            {
                LoadId = load.Id,
                OrderId = order.Id,
                PONumber = order.OrderNumber
            });

            await _loads.SaveChangesAsync();

            return load.Id;
        }





        //  ARCHIVE LOAD (Admin only)
        public async Task ArchiveAsync(Guid id, Guid userId)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new Exception("Load not found");

            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_Archive, load))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to archive this load.");

            load.IsArchived = true;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
        }

        // 🔒 PRIVATE HELPER
        private async Task<User> GetUserOrThrow(Guid userId)
        {
            return await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found.");
        }
    }
}
