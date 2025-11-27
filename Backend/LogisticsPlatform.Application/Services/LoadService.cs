using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services
{
    public class LoadService : ILoadService
    {
        private readonly ILoadRepository _loads;
        private readonly ICustomerRepository _customers;
        private readonly ICarrierRepository _carriers;

        public LoadService(
            ILoadRepository loads,
            ICustomerRepository customers,
            ICarrierRepository carriers)
        {
            _loads = loads;
            _customers = customers;
            _carriers = carriers;
        }

        public async Task<Guid> CreateAsync(CreateLoadDto dto, Guid createdByUserId)
        {
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

                CustomerRate = (decimal)dto.CustomerRate,
                CarrierRate = (decimal)dto.CarrierRate,
                Accessorials = dto.Accessorials,

                IsTemperatureControlled = dto.IsTemperatureControlled,
                IsArchived = false,

                CreatedByUserId = createdByUserId
            };

            await _loads.AddAsync(load);
            await _loads.SaveChangesAsync();

            return load.Id;
        }

        public async Task UpdateAsync(Guid id, UpdateLoadDto dto)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new Exception("Load not found");

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
                load.CarrierId = dto.CarrierId;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(Guid id, LoadStatus newStatus)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new Exception("Load not found");

            // business guardrails (opsionale)
            if (load.Status == LoadStatus.Completed)
                throw new Exception("Completed load cannot be changed");

            load.Status = newStatus;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
        }

        public async Task ArchiveAsync(Guid id)
        {
            var load = await _loads.GetByIdAsync(id)
                ?? throw new Exception("Load not found");

            load.IsArchived = true;

            await _loads.UpdateAsync(load);
            await _loads.SaveChangesAsync();
        }
    }
}
