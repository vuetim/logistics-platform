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
        private readonly ILoadFinancialAutomationService _financialAutomationService;

        public LoadService(
            ILoadRepository loads,
            ICustomerRepository customers,
            ICarrierRepository carriers,
            IUserRepository users,
            IAuthorizationService auth,
            IOrderRepository orders,
            ILoadFinancialAutomationService loadFinancialAutomationService)
        {
            _loads = loads;
            _customers = customers;
            _carriers = carriers;
            _users = users;
            _auth = auth;
            _orders = orders;
            _financialAutomationService = loadFinancialAutomationService;
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

        
        // UPDATE LOAD
        
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
            load.BolNumber = dto.BolNumber ?? load.BolNumber;
            load.ProNumber = dto.ProNumber ?? load.ProNumber;
            load.RateConfirmationNumber = dto.RateConfirmationNumber ?? load.RateConfirmationNumber;
            load.TrackingNumber = dto.TrackingNumber ?? load.TrackingNumber;

            load.DriverName = dto.DriverName ?? load.DriverName;
            load.DriverPhone = dto.DriverPhone ?? load.DriverPhone;
            load.DriverEmail = dto.DriverEmail ?? load.DriverEmail;

            load.TruckNumber = dto.TruckNumber ?? load.TruckNumber;
            load.TrailerNumber = dto.TrailerNumber ?? load.TrailerNumber;
            load.CarrierSCAC = dto.CarrierSCAC ?? load.CarrierSCAC;

            if (dto.PodReceivedAt.HasValue)
                load.PodReceivedAt = dto.PodReceivedAt.Value;
            if (!string.IsNullOrWhiteSpace(dto.PodUploadedBy))
                load.PodUploadedBy = dto.PodUploadedBy;


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
            if (newStatus == LoadStatus.Completed)
            {
                await _financialAutomationService.GenerateFinancialDocumentsAsync(load);
            }


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
            // 4 Snapshot OrderEquipmentRequirement → LoadEquipment
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

            if (load.Equipment.Any(e => e.EquipmentType == EquipmentType.Reefer))
            {
                load.IsTemperatureControlled = true;
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
//using LogisticsPlatform.Application.Authorization;
//using LogisticsPlatform.Application.Common.Exceptions;
//using LogisticsPlatform.Application.DTOs.Loads;
//using LogisticsPlatform.Application.Interfaces.Repositories;
//using LogisticsPlatform.Application.Interfaces.Services;
//using LogisticsPlatform.Domain.Entities;
//using LogisticsPlatform.Domain.Enums;
//using LogisticsPlatform.Domain.Security;
//using SendGrid.Helpers.Errors.Model;
//using ForbiddenException = LogisticsPlatform.Application.Common.Exceptions.ForbiddenException;

//namespace LogisticsPlatform.Application.Services;

//public class LoadService : ILoadService
//{
//    private readonly ILoadRepository _loads;
//    private readonly ICustomerRepository _customers;
//    private readonly ICarrierRepository _carriers;
//    private readonly IUserRepository _users;
//    private readonly IAuthorizationService _auth;
//    private readonly IOrderRepository _orders;
//    private readonly ILoadFinancialAutomationService _financialAutomation;

//    public LoadService(
//        ILoadRepository loads,
//        ICustomerRepository customers,
//        ICarrierRepository carriers,
//        IUserRepository users,
//        IAuthorizationService auth,
//        IOrderRepository orders,
//        ILoadFinancialAutomationService financialAutomation)
//    {
//        _loads = loads;
//        _customers = customers;
//        _carriers = carriers;
//        _users = users;
//        _auth = auth;
//        _orders = orders;
//        _financialAutomation = financialAutomation;
//    }

//    // ============================================================
//    // CREATE LOAD (Manual creation)
//    // ============================================================

//    public async Task<Guid> CreateAsync(CreateLoadDto dto, Guid userId)
//    {
//        var user = await GetUser(userId);

//        if (!_auth.HasPermission(user, Permission.Load_Create))
//            throw new ForbiddenException("You cannot create loads.");

//        var customer = await _customers.GetByIdAsync(dto.CustomerId)
//            ?? throw new NotFoundException("Customer not found.");

//        var load = new Load
//        {
//            LoadNumber = $"L-{DateTime.UtcNow:yyyyMMddHHmmss}",
//            CustomerId = customer.Id,
//            CarrierId = dto.CarrierId,

//            Origin = dto.Origin,
//            Destination = dto.Destination,

//            Mode = dto.Mode,
//            Status = LoadStatus.Draft,
//            IsTemperatureControlled = dto.IsTemperatureControlled,

//            CustomerRate = dto.CustomerRate,
//            CarrierRate = dto.CarrierRate,
//            Accessorials = dto.Accessorials,

//            CreatedByUserId = user.Id,
//            IsArchived = false
//        };

//        await _loads.AddAsync(load);
//        await _loads.SaveChangesAsync();

//        return load.Id;
//    }

//    // ============================================================
//    // UPDATE LOAD
//    // ============================================================

//    public async Task UpdateAsync(Guid id, UpdateLoadDto dto, Guid userId)
//    {
//        var load = await _loads.GetByIdAsync(id)
//            ?? throw new NotFoundException("Load not found.");

//        var user = await GetUser(userId);

//        if (!_auth.HasPermission(user, Permission.Load_Update, load))
//            throw new ForbiddenException("Not allowed to modify this load.");

//        load.Origin = dto.Origin ?? load.Origin;
//        load.Destination = dto.Destination ?? load.Destination;

//        if (dto.Mode.HasValue) load.Mode = dto.Mode.Value;
//        if (dto.EquipmentType.HasValue)
//            load.HasEquipment = true; // mark equipment presence

//        if (dto.CustomerRate.HasValue) load.CustomerRate = dto.CustomerRate.Value;
//        if (dto.CarrierRate.HasValue) load.CarrierRate = dto.CarrierRate.Value;
//        if (dto.Accessorials.HasValue) load.Accessorials = dto.Accessorials;

//        load.CarrierId = dto.CarrierId ?? load.CarrierId;

//        // Numbers
//        load.BolNumber = dto.BolNumber ?? load.BolNumber;
//        load.ProNumber = dto.ProNumber ?? load.ProNumber;
//        load.RateConfirmationNumber = dto.RateConfirmationNumber ?? load.RateConfirmationNumber;
//        load.TrackingNumber = dto.TrackingNumber ?? load.TrackingNumber;

//        // Driver
//        load.DriverName = dto.DriverName ?? load.DriverName;
//        load.DriverPhone = dto.DriverPhone ?? load.DriverPhone;
//        load.DriverEmail = dto.DriverEmail ?? load.DriverEmail;

//        load.TruckNumber = dto.TruckNumber ?? load.TruckNumber;
//        load.TrailerNumber = dto.TrailerNumber ?? load.TrailerNumber;
//        load.CarrierSCAC = dto.CarrierSCAC ?? load.CarrierSCAC;

//        if (dto.PodReceivedAt.HasValue)
//            load.PodReceivedAt = dto.PodReceivedAt.Value;

//        if (!string.IsNullOrWhiteSpace(dto.PodUploadedBy))
//            load.PodUploadedBy = dto.PodUploadedBy;

//        await _loads.UpdateAsync(load);
//        await _loads.SaveChangesAsync();
//    }

//    // ============================================================
//    // CHANGE STATUS (with financial automation)
//    // ============================================================

//    public async Task ChangeStatusAsync(Guid id, LoadStatus newStatus, Guid userId)
//    {
//        var load = await _loads.GetByIdAsync(id)
//            ?? throw new NotFoundException("Load not found.");

//        var user = await GetUser(userId);

//        if (!_auth.HasPermission(user, Permission.Load_ChangeStatus, load))
//            throw new ForbiddenException("You cannot change status.");

//        if (load.Status == LoadStatus.Completed)
//            throw new BusinessRuleException("Completed loads cannot be modified.");

//        // Auto-generate invoice/settlement when completed
//        if (newStatus == LoadStatus.Completed)
//            await _financialAutomation.GenerateFinancialDocumentsAsync(load);

//        load.Status = newStatus;

//        await _loads.UpdateAsync(load);
//        await _loads.SaveChangesAsync();
//    }

//    // ============================================================
//    // CREATE FROM ORDER (Snapshot Logic)
//    // ============================================================

//    public async Task<Guid> CreateFromOrderAsync(CreateLoadFromOrderDto dto, Guid userId)
//    {
//        var user = await GetUser(userId);

//        if (!_auth.HasPermission(user, Permission.Load_CreateFromOrder))
//            throw new ForbiddenException("Not allowed to create load from order.");

//        var order = await _orders.GetByIdWithRoutesAsync(dto.OrderId)
//            ?? throw new NotFoundException("Order not found.");

//        var routes = order.OrderRoutes
//            .Where(r => r.CopyToLoad && r.IsActive)
//            .OrderBy(r => r.Sequence)
//            .ToList();

//        if (!routes.Any())
//            throw new BusinessRuleException("Order has no routes.");

//        var first = routes.First();
//        var last = routes.Last();

//        var load = new Load
//        {
//            LoadNumber = $"L-{DateTime.UtcNow:yyyyMMddHHmmss}",
//            CustomerId = order.CustomerId,
//            CarrierId = dto.CarrierId ?? order.PreferredCarrierId,

//            Origin = $"{first.City}, {first.State}",
//            Destination = $"{last.City}, {last.State}",

//            Mode = ModeType.TL,
//            Status = LoadStatus.Draft,

//            CustomerRate = order.CustomerRate,
//            CarrierRate = dto.CarrierRate ?? 0,

//            CreatedByUserId = userId,
//            CreatedAt = DateTime.UtcNow,
//            IsArchived = false
//        };

//        await _loads.AddAsync(load);

//        // Stops snapshot
//        foreach (var r in routes)
//        {
//            await _loads.AddStopAsync(new LoadStop
//            {
//                Load = load,
//                Sequence = r.Sequence,
//                StopType = r.StopType,

//                LocationName = r.LocationName,
//                AddressLine1 = r.AddressLine1,
//                AddressLine2 = r.AddressLine2,
//                City = r.City,
//                State = r.State,
//                PostalCode = r.PostalCode,
//                Country = r.Country,

//                PlannedArrivalFrom = r.PlannedArrivalFrom,
//                PlannedArrivalTo = r.PlannedArrivalTo,
//                PlannedDepartureFrom = r.PlannedDepartureFrom,
//                PlannedDepartureTo = r.PlannedDepartureTo,

//                AppointmentType = r.AppointmentType,
//                FlexMinutes = r.FlexMinutes,
//                Notes = r.Notes,

//                Status = StopStatus.Pending
//            });
//        }

//        // Items snapshot
//        foreach (var item in order.Items)
//        {
//            load.Items.Add(new LoadItem
//            {
//                Load = load,
//                SourceOrderItemId = item.Id,
//                Name = item.Name,
//                Quantity = item.Quantity,
//                QuantityUnit = item.QuantityUnit,
//                IsHazmat = item.IsHazmat,
//                FreightClass = item.FreightClass,
//                Notes = item.Notes
//            });
//        }

//        // Equipment snapshot
//        foreach (var req in order.EquipmentRequirements.Where(e => e.CopyToLoad))
//        {
//            load.Equipment.Add(new LoadEquipment
//            {
//                Load = load,
//                EquipmentType = ParseEquipmentType(req.EquipmentType),
//                Length = ParseLength(req.EquipmentSize),
//                Weight = req.MaxWeight,
//                WeightUnit = WeightUnit.Lb,
//                MinTemp = req.RequiredTemperature,
//                MaxTemp = req.RequiredTemperature,
//                TempUnit = ParseTemperatureUnit(req.TemperatureUnit)
//            });
//        }

//        load.HasEquipment = load.Equipment.Any();
//        load.IsTemperatureControlled = load.Equipment.Any(e => e.EquipmentType == EquipmentType.Reefer);

//        // Link order
//        await _loads.AddLoadOrderAsync(new LoadOrder
//        {
//            Load = load,
//            Order = order,
//            PONumber = order.OrderNumber
//        });

//        await _loads.SaveChangesAsync();
//        return load.Id;
//    }

//    // ============================================================
//    // PRIVATE HELPERS
//    // ============================================================

//    private async Task<User> GetUser(Guid id)
//        => await _users.GetByIdAsync(id)
//        ?? throw new ForbiddenException("User not found.");

//    private EquipmentType ParseEquipmentType(string type)
//        => type?.ToLower() switch
//        {
//            "dry van" => EquipmentType.DryVan,
//            "reefer" => EquipmentType.Reefer,
//            "flatbed" => EquipmentType.Flatbed,
//            _ => EquipmentType.DryVan
//        };

//    private decimal? ParseLength(string? size)
//    {
//        if (string.IsNullOrWhiteSpace(size)) return null;
//        var digits = new string(size.Where(char.IsDigit).ToArray());
//        return decimal.TryParse(digits, out var v) ? v : null;
//    }

//    private TemperatureUnit ParseTemperatureUnit(string? unit)
//        => unit?.ToLower() switch
//        {
//            "c" => TemperatureUnit.C,
//            "celsius" => TemperatureUnit.C,
//            _ => TemperatureUnit.F
//        };
//}
