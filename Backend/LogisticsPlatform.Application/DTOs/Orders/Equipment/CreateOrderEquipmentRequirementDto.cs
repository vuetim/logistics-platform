using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Orders.Equipment
{
    public class CreateOrderEquipmentRequirementDto
    {
        public string EquipmentType { get; set; } = string.Empty;
        public string? EquipmentSize { get; set; }

        public decimal? MaxWeight { get; set; }
        public WeightUnit WeightUnit { get; set; }
        public decimal MinTemperature { get; set; }
        public decimal MaxTemperature { get; set; }
        public TemperatureUnit TemperatureUnit { get; set; }

        public int Quantity { get; set; } = 1;
        public bool IsMandatory { get; set; } = true;
        public bool CopyToLoad { get; set; } = true;
        public bool IsPrefered { get; set; } 
        public string? Notes { get; set; }
    }
}
