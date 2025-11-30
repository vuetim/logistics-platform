using LogisticsPlatform.Application.Authorization;
using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.Services
{
    public class LoadService : ILoadService
    {
        private readonly ILoadRepository _loads;
        private readonly ICustomerRepository _customers;
        private readonly ICarrierRepository _carriers;
        private readonly IUserRepository _users;
        private readonly IAuthorizationService _auth;

        public LoadService(
            ILoadRepository loads,
            ICustomerRepository customers,
            ICarrierRepository carriers,
            IUserRepository users,
            IAuthorizationService auth)
        {
            _loads = loads;
            _customers = customers;
            _carriers = carriers;
            _users = users;
            _auth = auth;
        }

        // ✅ CREATE LOAD (Admin, Broker)
        public async Task<Guid> CreateAsync(CreateLoadDto dto, Guid userId)
        {
            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_Create))
                throw new ForbiddenException("You are not allowed to create loads.");

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
                throw new ForbiddenException("You are not allowed to update this load.");

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
                throw new ForbiddenException("You are not allowed to change load status.");

            if (load.Status == LoadStatus.Completed)
                throw new Exception("Completed load cannot be changed.");

            load.Status = newStatus;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
        }

        //  ARCHIVE LOAD (Admin only)
        public async Task ArchiveAsync(Guid id, Guid userId)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new Exception("Load not found");

            var user = await GetUserOrThrow(userId);

            if (!_auth.HasPermission(user, Permission.Load_Archive, load))
                throw new ForbiddenException("You are not allowed to archive this load.");

            load.IsArchived = true;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
        }

        // 🔒 PRIVATE HELPER
        private async Task<User> GetUserOrThrow(Guid userId)
        {
            return await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found.");
        }
    }
}
