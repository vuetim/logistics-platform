using LogisticsPlatform.Application.DTOs.Loads.LoadEquipment;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
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
            TempUnit = e.TempUnit,
            Quantity = e.Quantity

        });
    }

    public async Task<LoadEquipmentDto> CreateAsync(Guid loadId, CreateLoadEquipmentDto dto)
    {
        var load = await _loadRepo.GetByIdAsync(loadId);
        if (load == null)
            throw new Exception("Load not found");

        // Reefer validation
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
            TempUnit = dto.TempUnit,
            Quantity = dto.Quantity
        };

        await _equipmentRepo.AddAsync(equipment);

        // 🔥 KËTU — vendosim flag-un në Load
        load.HasEquipment = true;
        await _loadRepo.UpdateAsync(load);

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
            TempUnit = equipment.TempUnit,
            Quantity = equipment.Quantity,

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
        equipment.Quantity = dto.Quantity;

        await _equipmentRepo.UpdateAsync(equipment);
        await _equipmentRepo.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var equipment = await _equipmentRepo.GetByIdAsync(id)
            ?? throw new Exception("Equipment not found");

        var load = await _loadRepo.GetByIdAsync(equipment.LoadId)
            ?? throw new Exception("Load not found");

        await _equipmentRepo.DeleteAsync(equipment);

        var remaining = await _equipmentRepo.GetByLoadIdAsync(load.Id);

        if (!remaining.Any())
        {
            load.HasEquipment = false;
            load.IsTemperatureControlled = false;
            await _loadRepo.UpdateAsync(load);
        }
        else if (!remaining.Any(e => e.EquipmentType == EquipmentType.Reefer))
        {
            load.IsTemperatureControlled = false;
            await _loadRepo.UpdateAsync(load);
        }

        await _equipmentRepo.SaveChangesAsync();
    }

}
