using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
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
        private readonly IOrderLoadSyncService _orderLoadSyncService;
        private readonly ILoadCreationPolicy _creationPolicy;
        private readonly ILoadDispatchPolicy _dispatchPolicy;
        private readonly IOrderToLoadRouteSelector _routeSelector;
        private readonly IOrderToLoadSnapshotBuilder _snapshotBuilder;
        private readonly ILoadNumberGenerator _numberGenerator;

        public LoadService(
            ILoadRepository loads,
            ICustomerRepository customers,
            ICarrierRepository carriers,
            IUserRepository users,
            IPermissionService permission,
            IOrderRepository orders,
            ILoadFinancialAutomationService loadFinancialAutomationService,
            IOrderLoadSyncService orderLoadSyncService,
            ILoadCreationPolicy creationPolicy,
            ILoadDispatchPolicy dispatchPolicy,
            IOrderToLoadRouteSelector routeSelector,
            IOrderToLoadSnapshotBuilder snapshotBuilder,
            ILoadNumberGenerator numberGenerator)
        {
            _loads = loads;
            _customers = customers;
            _carriers = carriers;
            _users = users;
            _permission = permission;
            _orders = orders;
            _financialAutomationService = loadFinancialAutomationService;
            _orderLoadSyncService = orderLoadSyncService;
            _creationPolicy = creationPolicy;
            _dispatchPolicy = dispatchPolicy;
            _routeSelector = routeSelector;
            _snapshotBuilder = snapshotBuilder;
            _numberGenerator = numberGenerator;
        }

        // CREATE LOAD (manual) (Admin, Broker)
        
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
                LoadNumber = _numberGenerator.Generate(),

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

            var canUpdateLoad = await _permission.HasPermissionAsync(userId, Permission.Load_Update);
            var canUpdateOperational = await _permission.HasPermissionAsync(userId, Permission.Load_Operational_Update);

            if (!canUpdateLoad && !canUpdateOperational)
                throw new Common.Exceptions.ForbiddenException("You are not allowed to update this load.");

            if (!canUpdateLoad && HasProtectedLoadChanges(dto))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to update load commercial or assignment fields.");

            load.Origin = dto.Origin ?? load.Origin;
            load.Destination = dto.Destination ?? load.Destination;

            if (canUpdateLoad && dto.ModeType.HasValue)
                load.Mode = dto.ModeType.Value;

            if (canUpdateLoad && dto.CustomerRate.HasValue)
                load.CustomerRate = dto.CustomerRate.Value;

            if (canUpdateLoad && dto.CarrierRate.HasValue)
                load.CarrierRate = dto.CarrierRate.Value;

            if (canUpdateLoad)
                load.Accessorials = dto.Accessorials;

            if (canUpdateLoad && dto.CarrierId.HasValue)
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

        // CHANGE LOAD STATUS
        public async Task ChangeStatusAsync(Guid id, LoadStatus newStatus, Guid userId)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new NotFoundException("Load not found.");

            var user = await GetUserOrThrow(userId);

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_ChangeStatus))
                throw new Common.Exceptions.ForbiddenException("You are not allowed to change load status.");

            if (newStatus == LoadStatus.Completed)
            {
                if (load.Status == LoadStatus.Completed)
                    throw new BusinessRuleException("Load is already completed.");

                if (load.Status != LoadStatus.Delivered)
                    throw new BusinessRuleException("Load must be delivered before it can be completed.");
            }
            else if (load.Status == LoadStatus.Completed)
            {
                throw new BusinessRuleException("Completed load cannot be changed.");
            }


            load.Status = newStatus;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
            await _orderLoadSyncService.SyncFromLoadAsync(load);

            if (newStatus == LoadStatus.Completed)
            {
                await _financialAutomationService.GenerateFinancialDocumentsAsync(load);
            }
        }

        // CREATE LOAD FROM ORDER (snapshot)
        public async Task<Guid> CreateFromOrderAsync(
            CreateLoadFromOrderDto dto,
            Guid userId)
        {
            _creationPolicy.ValidateDateOverrides(dto.PlannedPickupDate, dto.PlannedDeliveryDate);

            var user = await GetUserOrThrow(userId);

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_CreateFromOrder))
                throw new Common.Exceptions.ForbiddenException("Not allowed to create load from order.");

            var order = await _orders.GetByIdWithRoutesAsync(dto.OrderId)
                ?? throw new NotFoundException("Order not found.");

            var orderWithLoads = await _orders.GetByIdWithLoadsAsync(dto.OrderId)
                ?? throw new NotFoundException("Order not found.");

            _creationPolicy.EnsureCanCreateFromOrder(order, orderWithLoads, dto);

            var routes = _routeSelector.Select(order);
            var snapshot = _snapshotBuilder.Build(order, dto, user.Id, routes);

            await _loads.AddAsync(snapshot.Load);

            foreach (var stop in snapshot.Stops)
            {
                await _loads.AddStopAsync(stop);
            }

            await _loads.AddLoadOrderAsync(snapshot.LoadOrder);
            await _loads.SaveChangesAsync();
            await _orderLoadSyncService.SyncByOrderIdAsync(order.Id);

            return snapshot.Load.Id;
        }

        // ARCHIVE LOAD (Admin only)
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

        // PRIVATE HELPERS
        private async Task<User> GetUserOrThrow(Guid userId)
        {
            return await _users.GetByIdAsync(userId)
                ?? throw new Common.Exceptions.ForbiddenException("User not found.");
        }

        private static bool HasProtectedLoadChanges(UpdateLoadDto dto)
        {
            return dto.CarrierId.HasValue
                || dto.ModeType.HasValue
                || dto.EquipmentType.HasValue
                || dto.PickupDate.HasValue
                || dto.DeliveryDate.HasValue
                || dto.CustomerRate.HasValue
                || dto.CarrierRate.HasValue
                || dto.Accessorials.HasValue
                || !string.IsNullOrWhiteSpace(dto.Summary);
        }

        public async Task DispatchAsync(Guid loadId, DispatchLoadDto dto, Guid userId)
        {
            var load = await _loads.GetByIdAsync(loadId)
                ?? throw new NotFoundException("Load not found.");

            var user = await GetUserOrThrow(userId);

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Dispatch))
                throw new ForbiddenException("You are not allowed to dispatch this load.");

            _dispatchPolicy.EnsureCanDispatch(load, dto);

            // Snapshot dispatcher data
            load.DriverName = dto.DriverName;
            load.DriverPhone = dto.DriverPhone;
            load.DriverEmail = dto.DriverEmail;

            load.TruckNumber = dto.TruckNumber;
            load.TrailerNumber = dto.TrailerNumber;

            load.Status = LoadStatus.Dispatched;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
            await _orderLoadSyncService.SyncFromLoadAsync(load);
        }

    }
}

       


