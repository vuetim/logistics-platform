using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Orders.Equipment;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Domain.Entities;
using SendGrid.Helpers.Errors.Model;

namespace LogisticsPlatform.Application.Services.Orders
{
    public class OrderEquipmentRequirementService : IOrderEquipmentRequirementService
    {
        private readonly IOrderEquipmentRequirementRepository _equipmentRepo;
        private readonly IOrderRepository _orderRepo;

        public OrderEquipmentRequirementService(
            IOrderEquipmentRequirementRepository equipmentRepo,
            IOrderRepository orderRepo)
        {
            _equipmentRepo = equipmentRepo;
            _orderRepo = orderRepo;
        }

        public async Task<IEnumerable<OrderEquipmentRequirementDto>> GetByOrderAsync(Guid orderId)
        {
            var requirements = await _equipmentRepo.GetByOrderIdAsync(orderId);

            return requirements.Select(r => new OrderEquipmentRequirementDto
            {
                Id = r.Id,
                EquipmentType = r.EquipmentType,
                EquipmentSize = r.EquipmentSize,

                Quantity = r.Quantity,

                MaxWeight = r.MaxWeight,
                WeightUnit = r.WeightUnit,

                MinTemperature = r.MinTemperature,
                MaxTemperature = r.MaxTemperature,
                TemperatureUnit = r.TemperatureUnit,

                IsMandatory = r.IsMandatory,
                IsPrefered = r.IsPrefered,

                CopyToLoad = r.CopyToLoad,
                Notes = r.Notes
            });
        }

        public async Task<OrderEquipmentRequirementDto> CreateAsync(
            Guid orderId,
            CreateOrderEquipmentRequirementDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(orderId)
                ?? throw new NotFoundException("Order not found.");

            var req = new OrderEquipmentRequirement
            {
                OrderId = orderId,

                EquipmentType = dto.EquipmentType,
                EquipmentSize = dto.EquipmentSize,

                Quantity = dto.Quantity,

                MaxWeight = dto.MaxWeight,
                WeightUnit = dto.WeightUnit,

                MinTemperature = dto.MinTemperature,
                MaxTemperature = dto.MaxTemperature,
                TemperatureUnit = dto.TemperatureUnit,

                IsMandatory = dto.IsMandatory,
                IsPrefered = dto.IsPrefered,

                CopyToLoad = dto.CopyToLoad,
                Notes = dto.Notes
            };

            await _equipmentRepo.AddAsync(req);
            await _equipmentRepo.SaveChangesAsync();

            return new OrderEquipmentRequirementDto
            {
                Id = req.Id,
                EquipmentType = req.EquipmentType,
                EquipmentSize = req.EquipmentSize,

                Quantity = req.Quantity,

                MaxWeight = req.MaxWeight,
                WeightUnit = req.WeightUnit,

                MinTemperature = req.MinTemperature,
                MaxTemperature = req.MaxTemperature,
                TemperatureUnit = req.TemperatureUnit,

                IsMandatory = req.IsMandatory,
                IsPrefered = req.IsPrefered,

                CopyToLoad = req.CopyToLoad,
                Notes = req.Notes
            };
        }

        public async Task UpdateAsync(Guid id, UpdateOrderEquipmentRequirementDto dto)
        {
            var req = await _equipmentRepo.GetByIdAsync(id)
                ?? throw new NotFoundException("Equipment requirement not found.");

            req.EquipmentType = dto.EquipmentType;
            req.EquipmentSize = dto.EquipmentSize;

            req.Quantity = dto.Quantity;

            req.MaxWeight = dto.MaxWeight;
            req.WeightUnit = dto.WeightUnit;

            req.MinTemperature = dto.MinTemperature;
            req.MaxTemperature = dto.MaxTemperature;
            req.TemperatureUnit = dto.TemperatureUnit;

            req.IsMandatory = dto.IsMandatory;
            req.IsPrefered = dto.IsPrefered;

            req.CopyToLoad = dto.CopyToLoad;
            req.Notes = dto.Notes;
            await _equipmentRepo.UpdateAsync(req);
            await _equipmentRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var req = await _equipmentRepo.GetByIdAsync(id)
                ?? throw new NotFoundException("Equipment requirement not found.");

            await _equipmentRepo.DeleteAsync(req);
            await _equipmentRepo.SaveChangesAsync();
        }
    }
}
