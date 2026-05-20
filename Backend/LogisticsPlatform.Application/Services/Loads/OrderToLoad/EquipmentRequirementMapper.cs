using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services.Loads.OrderToLoad;

public static class EquipmentRequirementMapper
{
    public static LoadEquipment ToLoadEquipment(OrderEquipmentRequirement requirement, Load load)
        => new()
        {
            Load = load,
            SourceOrderEquipmentRequirementId = requirement.Id,
            EquipmentType = ParseEquipmentType(requirement.EquipmentType),
            Quantity = requirement.Quantity > 0 ? requirement.Quantity : 1,
            Length = ParseLength(requirement.EquipmentSize),
            Weight = requirement.MaxWeight,
            WeightUnit = requirement.WeightUnit,
            MinTemp = requirement.MinTemperature,
            MaxTemp = requirement.MaxTemperature,
            TempUnit = requirement.TemperatureUnit,
            IsPrefered = requirement.IsPrefered
        };

    private static EquipmentType ParseEquipmentType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return EquipmentType.DryVan;

        return type.ToLowerInvariant() switch
        {
            "dry van" => EquipmentType.DryVan,
            "van" => EquipmentType.DryVan,
            "reefer" => EquipmentType.Reefer,
            "refrigerated" => EquipmentType.Reefer,
            "flatbed" => EquipmentType.Flatbed,
            "stepdeck" => EquipmentType.StepDeck,
            "step deck" => EquipmentType.StepDeck,
            "power only" => EquipmentType.PowerOnly,
            _ => EquipmentType.DryVan
        };
    }

    private static decimal? ParseLength(string? size)
    {
        if (string.IsNullOrWhiteSpace(size))
            return null;

        var clean = new string(size.Where(char.IsDigit).ToArray());

        return decimal.TryParse(clean, out var length)
            ? length
            : null;
    }
}

