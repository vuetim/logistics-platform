using LogisticsPlatform.Application.DTOs.Loads.LoadEquipment;

namespace LogisticsPlatform.Application.Interfaces.Services;

public interface ILoadEquipmentService
{
    Task<IEnumerable<LoadEquipmentDto>> GetByLoadAsync(Guid loadId);
    Task<LoadEquipmentDto> CreateAsync(Guid loadId, CreateLoadEquipmentDto dto);
    Task UpdateAsync(Guid id, UpdateLoadEquipmentDto dto);
    Task DeleteAsync(Guid id);
}
