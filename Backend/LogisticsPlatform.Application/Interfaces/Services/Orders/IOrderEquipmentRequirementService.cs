using LogisticsPlatform.Application.DTOs.Orders.Equipment;

namespace LogisticsPlatform.Application.Interfaces.Services.Orders
{
    public interface IOrderEquipmentRequirementService
    {
        Task<IEnumerable<OrderEquipmentRequirementDto>> GetByOrderAsync(Guid orderId);
        Task<OrderEquipmentRequirementDto> CreateAsync(Guid orderId, CreateOrderEquipmentRequirementDto dto);
        Task UpdateAsync(Guid id, UpdateOrderEquipmentRequirementDto dto);
        Task DeleteAsync(Guid id);
    }
}
