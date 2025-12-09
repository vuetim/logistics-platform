namespace LogisticsPlatform.Application.DTOs.Orders.Equipment
{
    public class CreateOrderEquipmentRequirementDto
    {
        public string EquipmentType { get; set; } = string.Empty;
        public string? EquipmentSize { get; set; }

        public decimal? MaxWeight { get; set; }
        public string? WeightUnit { get; set; }

        public decimal? RequiredTemperature { get; set; }
        public string? TemperatureUnit { get; set; }

        public int Quantity { get; set; } = 1;
        public bool IsMandatory { get; set; } = true;
        public bool CopyToLoad { get; set; } = true;

        public string? Notes { get; set; }
    }
}
