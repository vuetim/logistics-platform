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

        // =============================
        // CREATE LOAD (manual) (Admin, Broker)
        // =============================
        public async Task<Guid> CreateAsync(CreateLoadDto dto, Guid userId)
        {
            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_Create))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to create loads.");

            var customer = await _customers.GetByIdAsync(dto.CustomerId)
                ?? throw new NotFoundException("Customer not found.");

            Carrier? carrier = null;
            if (dto.CarrierId.HasValue)
            {
                carrier = await _carriers.GetByIdAsync(dto.CarrierId.Value)
                    ?? throw new NotFoundException("Carrier not found.");
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

        // =============================
        // UPDATE LOAD
        // =============================
        public async Task UpdateAsync(Guid id, UpdateLoadDto dto, Guid userId)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new NotFoundException("Load not found.");

            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_Update, load))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to update this load.");

            load.Origin = dto.Origin ?? load.Origin;
            load.Destination = dto.Destination ?? load.Origin;

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

        // =============================
        // CHANGE LOAD STATUS
        // =============================
        public async Task ChangeStatusAsync(Guid id, LoadStatus newStatus, Guid userId)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new NotFoundException("Load not found.");

            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_ChangeStatus, load))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to change load status.");

            if (load.Status == LoadStatus.Completed)
                throw new BusinessRuleException("Completed load cannot be changed.");

            load.Status = newStatus;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
        }

        // =============================
        // CREATE LOAD FROM ORDER (snapshot)
        // =============================
        public async Task<Guid> CreateFromOrderAsync(
            CreateLoadFromOrderDto dto,
            Guid userId)
        {
            // 1️⃣ Load user & permissions
            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_CreateFromOrder))
                throw new Common.Exceptions.ForbiddenException("Not allowed to create load from order.");

            // 2️⃣ Load order with routes + items
            var order = await _orders.GetByIdWithRoutesAsync(dto.OrderId)
                ?? throw new NotFoundException("Order not found.");

            // SHËNIM:
            // GetByIdWithRoutesAsync duhet të përfshij:
            //  .Include(o => o.OrderRoutes)
            //  .Include(o => o.Items)

            var routes = order.OrderRoutes
                .Where(r => r.CopyToLoad && r.IsActive)
                .OrderBy(r => r.Sequence)
                .ToList();

            if (!routes.Any())
                throw new BusinessRuleException("No active routes to copy.");

            // (opsionale) nëse biznesi kërkon patjetër items:
            // if (!order.Items.Any())
            //     throw new BusinessRuleException("Order has no items.");

            // 3️⃣ Create Load (in-memory, pa SaveChanges ende)
            var firstRoute = routes.First();
            var lastRoute = routes.Last();

            var load = new Load
            {
                LoadNumber = $"L-{DateTime.UtcNow:yyyyMMddHHmmss}",

                CustomerId = order.CustomerId,
                CarrierId = dto.CarrierId ?? order.PreferredCarrierId,

                Status = LoadStatus.Draft,
                Mode = ModeType.TL, // TODO: më vonë mund të vijë nga Order/DTO

                CustomerRate = order.CustomerRate,
                CarrierRate = dto.CarrierRate ?? 0,

                Origin = $"{firstRoute.City}, {firstRoute.State}",
                Destination = $"{lastRoute.City}, {lastRoute.State}",

                IsArchived = false,
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _loads.AddAsync(load);

            //  Snapshot OrderRoutes - LoadStops
            foreach (var route in routes)
            {
                var stop = new LoadStop
                {
                    // lidhje me Load si navigation – EF do mbush LoadId
                    Load = load,

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
                };

                await _loads.AddStopAsync(stop);
            }

            //  Snapshot OrderItems - LoadItems
            foreach (var orderItem in order.Items)
            {
                var loadItem = new LoadItem
                {
                    Load = load,                   // navigation
                    SourceOrderItemId = orderItem.Id,

                    Name = orderItem.Name,
                    Quantity = orderItem.Quantity,
                    QuantityUnit = orderItem.QuantityUnit,

                    IsHazmat = orderItem.IsHazmat,
                    FreightClass = orderItem.FreightClass,
                    Notes = orderItem.Notes
                };

                load.Items.Add(loadItem);
            }
            // 4️⃣ Snapshot OrderEquipmentRequirement → LoadEquipment
            var equipmentReqs = order.EquipmentRequirements
                .Where(e => e.CopyToLoad)
                .ToList();

            foreach (var req in equipmentReqs)
            {
                var loadEq = new LoadEquipment
                {
                    Load = load,

                    // Convert string → EquipmentType enum
                    EquipmentType = ParseEquipmentType(req.EquipmentType),

                    // Convert "53 ft" → 53
                    Length = ParseLength(req.EquipmentSize),

                    // Snapshot weight requirement
                    Weight = req.MaxWeight,
                    WeightUnit = WeightUnit.Lb, //  WeightUnit.Kg or req.WeightUnit = "kg"

                    // Temperature snapshot only if Reefer
                    MinTemp = req.RequiredTemperature,
                    MaxTemp = req.RequiredTemperature,
                    TempUnit = ParseTemperatureUnit(req.TemperatureUnit)
                };

                load.Equipment.Add(loadEq);
            }
            if (load.Equipment.Any())
            {
                load.HasEquipment = true;
            }

            // Link Order ↔ Load (LoadOrder)
            var loadOrder = new LoadOrder
            {
                Load = load,
                OrderId = order.Id,
                PONumber = order.OrderNumber
            };

            await _loads.AddLoadOrderAsync(loadOrder);

            // one SaveChanges for all
            await _loads.SaveChangesAsync();

            return load.Id;
        }

        // =============================
        // ARCHIVE LOAD (Admin only)
        // =============================
        public async Task ArchiveAsync(Guid id, Guid userId)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new NotFoundException("Load not found.");

            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_Archive, load))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to archive this load.");

            load.IsArchived = true;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
        }

        // =============================
        // PRIVATE HELPERS
        // =============================
        private async Task<User> GetUserOrThrow(Guid userId)
        {
            return await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found.");
        }

        private EquipmentType ParseEquipmentType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return EquipmentType.DryVan;

            return type.ToLower() switch
            {
                "dry van" => EquipmentType.DryVan,
                "van" => EquipmentType.DryVan,
                "reefer" => EquipmentType.Reefer,
                "refrigerated" => EquipmentType.Reefer,
                "flatbed" => EquipmentType.Flatbed,
                "stepdeck" => EquipmentType.StepDeck,
                "step deck" => EquipmentType.StepDeck,
                "power only" => EquipmentType.PowerOnly,
                _ => EquipmentType.DryVan
            };
        }

        private decimal? ParseLength(string? size)
        {
            if (string.IsNullOrWhiteSpace(size))
                return null;

            // Examples: "53 ft", "48ft", "53"
            var clean = new string(size.Where(char.IsDigit).ToArray());

            if (decimal.TryParse(clean, out var length))
                return length;

            return null;
        }

        private TemperatureUnit ParseTemperatureUnit(string? unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
                return TemperatureUnit.F;

            return unit.ToLower() switch
            {
                "f" => TemperatureUnit.F,
                "fahrenheit" => TemperatureUnit.F,
                "c" => TemperatureUnit.C,
                "celsius" => TemperatureUnit.C,
                _ => TemperatureUnit.F
            };
        }

    }
}
