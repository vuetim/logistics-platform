using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Orders
{
    public interface IOrderEquipmentRequirementRepository
    {
        Task AddAsync(OrderEquipmentRequirement requirement);
        Task UpdateAsync(OrderEquipmentRequirement requirement);
        Task DeleteAsync(OrderEquipmentRequirement requirement);
        Task<OrderEquipmentRequirement?> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderEquipmentRequirement>> GetByOrderIdAsync(Guid orderId);
        Task SaveChangesAsync();
    }
}
