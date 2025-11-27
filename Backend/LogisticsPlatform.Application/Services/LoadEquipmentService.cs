using LogisticsPlatform.Application.DTOs.Loads.LoadEquipment;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services;

public class LoadEquipmentService : ILoadEquipmentService
{
    private readonly ILoadEquipmentRepository _equipmentRepo;
    private readonly ILoadRepository _loadRepo;

    public LoadEquipmentService(
        ILoadEquipmentRepository equipmentRepo,
        ILoadRepository loadRepo)
    {
        _equipmentRepo = equipmentRepo;
        _loadRepo = loadRepo;
    }

    public async Task<IEnumerable<LoadEquipmentDto>> GetByLoadAsync(Guid loadId)
    {
        var equipment = await _equipmentRepo.GetByLoadIdAsync(loadId);

        return equipment.Select(e => new LoadEquipmentDto
        {
            Id = e.Id,
            EquipmentType = e.EquipmentType,
            Length = e.Length,
            Weight = e.Weight,
            WeightUnit = e.WeightUnit,
            MinTemp = e.MinTemp,
            MaxTemp = e.MaxTemp,
            TempUnit = e.TempUnit
        });
    }

    public async Task<LoadEquipmentDto> CreateAsync(Guid loadId, CreateLoadEquipmentDto dto)
    {
        var load = await _loadRepo.GetByIdAsync(loadId);
        if (load == null)
            throw new Exception("Load not found");

        // ✅ Reefer validation
        if (dto.EquipmentType == EquipmentType.Reefer &&
            (!dto.MinTemp.HasValue || !dto.MaxTemp.HasValue))
        {
            throw new Exception("Temperature is required for reefer equipment");
        }

        var equipment = new LoadEquipment
        {
            LoadId = loadId,
            EquipmentType = dto.EquipmentType,
            Length = dto.Length,
            Weight = dto.Weight,
            WeightUnit = dto.WeightUnit,
            MinTemp = dto.MinTemp,
            MaxTemp = dto.MaxTemp,
            TempUnit = dto.TempUnit
        };

        await _equipmentRepo.AddAsync(equipment);
        await _equipmentRepo.SaveChangesAsync();

        return new LoadEquipmentDto
        {
            Id = equipment.Id,
            EquipmentType = equipment.EquipmentType,
            Length = equipment.Length,
            Weight = equipment.Weight,
            WeightUnit = equipment.WeightUnit,
            MinTemp = equipment.MinTemp,
            MaxTemp = equipment.MaxTemp,
            TempUnit = equipment.TempUnit
        };
    }

    public async Task UpdateAsync(Guid id, UpdateLoadEquipmentDto dto)
    {
        var equipment = await _equipmentRepo.GetByIdAsync(id)
            ?? throw new Exception("Equipment not found");

        equipment.EquipmentType = dto.EquipmentType;
        equipment.Length = dto.Length;
        equipment.Weight = dto.Weight;
        equipment.WeightUnit = dto.WeightUnit;
        equipment.MinTemp = dto.MinTemp;
        equipment.MaxTemp = dto.MaxTemp;
        equipment.TempUnit = dto.TempUnit;

        await _equipmentRepo.UpdateAsync(equipment);
        await _equipmentRepo.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var equipment = await _equipmentRepo.GetByIdAsync(id)
            ?? throw new Exception("Equipment not found");

        await _equipmentRepo.DeleteAsync(equipment);
        await _equipmentRepo.SaveChangesAsync();
    }
}
