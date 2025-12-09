using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Orders.Equipment;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
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
                MaxWeight = r.MaxWeight,
                WeightUnit = r.WeightUnit,
                RequiredTemperature = r.RequiredTemperature,
                TemperatureUnit = r.TemperatureUnit,
                Quantity = r.Quantity,
                IsMandatory = r.IsMandatory,
                CopyToLoad = r.CopyToLoad,
                Notes = r.Notes
            });
        }

        public async Task<OrderEquipmentRequirementDto> CreateAsync(Guid orderId, CreateOrderEquipmentRequirementDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(orderId)
                ?? throw new NotFoundException("Order not found.");

            var req = new OrderEquipmentRequirement
            {
                OrderId = orderId,
                EquipmentType = dto.EquipmentType,
                EquipmentSize = dto.EquipmentSize,
                MaxWeight = dto.MaxWeight,
                WeightUnit = dto.WeightUnit,
                RequiredTemperature = dto.RequiredTemperature,
                TemperatureUnit = dto.TemperatureUnit,
                Quantity = dto.Quantity,
                IsMandatory = dto.IsMandatory,
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
                MaxWeight = req.MaxWeight,
                WeightUnit = req.WeightUnit,
                RequiredTemperature = req.RequiredTemperature,
                TemperatureUnit = req.TemperatureUnit,
                Quantity = req.Quantity,
                IsMandatory = req.IsMandatory,
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
            req.MaxWeight = dto.MaxWeight;
            req.WeightUnit = dto.WeightUnit;
            req.RequiredTemperature = dto.RequiredTemperature;
            req.TemperatureUnit = dto.TemperatureUnit;
            req.Quantity = dto.Quantity;
            req.IsMandatory = dto.IsMandatory;
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
