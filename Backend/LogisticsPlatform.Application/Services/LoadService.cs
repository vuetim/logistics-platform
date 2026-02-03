using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
using SendGrid.Helpers.Errors.Model;
using ForbiddenException = LogisticsPlatform.Application.Common.Exceptions.ForbiddenException;

namespace LogisticsPlatform.Application.Services
{
    public class LoadService : ILoadService
    {
        private readonly ILoadRepository _loads;
        private readonly ICustomerRepository _customers;
        private readonly ICarrierRepository _carriers;
        private readonly IUserRepository _users;
        private readonly IPermissionService _permission;
        private readonly IOrderRepository _orders;
        private readonly ILoadFinancialAutomationService _financialAutomationService;

        public LoadService(
            ILoadRepository loads,
            ICustomerRepository customers,
            ICarrierRepository carriers,
            IUserRepository users,
            IPermissionService permission,
            IOrderRepository orders,
            ILoadFinancialAutomationService loadFinancialAutomationService)
        {
            _loads = loads;
            _customers = customers;
            _carriers = carriers;
            _users = users;
            _permission = permission;
            _orders = orders;
            _financialAutomationService = loadFinancialAutomationService;
        }

        // =============================
        // CREATE LOAD (manual) (Admin, Broker)
        // =============================
        public async Task<Guid> CreateAsync(CreateLoadDto dto, Guid userId)
        {
            var user = await GetUserOrThrow(userId);

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Create))
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

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Update))
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

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_ChangeStatus))
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

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_CreateFromOrder))
                throw new Common.Exceptions.ForbiddenException("Not allowed to create load from order.");

            // 2️⃣ Load order with routes + items
            var order = await _orders.GetByIdWithRoutesAsync(dto.OrderId)
                ?? throw new NotFoundException("Order not found.");

            // NOTE:
            // GetByIdWithRoutesAsync duhet të përfshij:
            //  .Include(o => o.OrderRoutes)
            //  .Include(o => o.Items)

            var routes = order.OrderRoutes
                .Where(r => r.CopyToLoad && r.IsActive)
                .OrderBy(r => r.Sequence)
                .ToList();

            if (!routes.Any())
                throw new BusinessRuleException("No active routes to copy.");

            // (opsionale) nese biznesi kerkon patjeter items:
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
                Mode = ModeType.TL, // TODO: me vone mund të vije nga Order/DTO

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
                    StopReference = route.StopReference,
                    AppointmentNumber = route.AppointmentNumber,
                    Status = StopStatus.Pending,
                    Notes = route.Notes
                };

                await _loads.AddStopAsync(stop);
            }

            //  Snapshot OrderItems - LoadItems
            foreach (var orderItem in order.Items.Where(i => i.CopyToLoad))

            {
                var loadItem = new LoadItem
                {
                    Load = load,
                    SourceOrderItemId = orderItem.Id,

                    // Identification
                    Name = orderItem.Name,
                    CustomerReference = orderItem.CustomerReference,

                    // Quantity & handling
                    Quantity = orderItem.Quantity,
                    QuantityUnit = orderItem.QuantityUnit,
                    HandlingQuantity = orderItem.HandlingQuantity,
                    HandlingUnit = orderItem.HandlingUnit,

                    // Weight
                    UnitNetWeight = orderItem.UnitNetWeight,
                    UnitGrossWeight = orderItem.UnitGrossWeight,
                    WeightUnit = orderItem.WeightUnit,

                    // Dimensions
                    Length = orderItem.Length,
                    Width = orderItem.Width,
                    Height = orderItem.Height,
                    DimensionUnit = orderItem.DimensionUnit,

                    // Volume
                    Volume = orderItem.Volume,
                    VolumeUnit = orderItem.VolumeUnit,

                    // Temperature
                    MinTemperature = orderItem.MinTemperature,
                    MaxTemperature = orderItem.MaxTemperature,
                    TemperatureUnit = orderItem.TemperatureUnit,

                    // Hazmat
                    IsHazmat = orderItem.IsHazmat,
                    HazardClass = orderItem.HazardClass,
                    IdentificationNumber = orderItem.IdentificationNumber,

                    // Freight & commercial
                    FreightClass = orderItem.FreightClass,
                    DeclaredValue = orderItem.DeclaredValue,
                    Currency = orderItem.Currency,

                    Stackable = orderItem.Stackable,
                    Notes = orderItem.Notes
                };

                load.Items.Add(loadItem);
            }
            // 4 Snapshot OrderEquipmentRequirement → LoadEquipment
            var equipmentReqs = order.EquipmentRequirements
                .Where(e => e.CopyToLoad)
                .ToList();

            foreach (var req in order.EquipmentRequirements.Where(e => e.CopyToLoad))
            {
                var loadEq = new LoadEquipment
                {
                    Load = load,
                    SourceOrderEquipmentRequirementId = req.Id,

                    EquipmentType = ParseEquipmentType(req.EquipmentType),
                    Quantity = req.Quantity > 0 ? req.Quantity : 1,

                    Length = ParseLength(req.EquipmentSize),

                    Weight = req.MaxWeight,
                    WeightUnit = req.WeightUnit,

                    MinTemp = req.MinTemperature,
                    MaxTemp = req.MaxTemperature,
                    TempUnit = req.TemperatureUnit,
                    IsPrefered = req.IsPrefered,
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

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Archive))
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
        public async Task DispatchAsync(Guid loadId, DispatchLoadDto dto, Guid userId)
        {
            var load = await _loads.GetByIdAsync(loadId)
                ?? throw new NotFoundException("Load not found.");

            var user = await GetUserOrThrow(userId);

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Dispatch))
                throw new ForbiddenException("You are not allowed to dispatch this load.");

            // 🔒 Business rules
            if (load.Status != LoadStatus.Accepted)
                throw new BusinessRuleException("Only accepted loads can be dispatched.");

            if (load.CarrierId == null)
                throw new BusinessRuleException("Carrier must be assigned before dispatch.");

            // Snapshot dispatcher data
            load.DriverName = dto.DriverName;
            load.DriverPhone = dto.DriverPhone;
            load.DriverEmail = dto.DriverEmail;

            load.TruckNumber = dto.TruckNumber;
            load.TrailerNumber = dto.TrailerNumber;

            load.Status = LoadStatus.Dispatched;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
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

       


