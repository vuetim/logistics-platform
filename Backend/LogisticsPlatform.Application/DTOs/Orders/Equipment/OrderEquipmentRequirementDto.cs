namespace LogisticsPlatform.Application.DTOs.Orders.Equipment
{
    public class OrderEquipmentRequirementDto
    {
        public Guid Id { get; set; }

        public string EquipmentType { get; set; } = string.Empty; // "Dry Van", "Reefer"
        public string? EquipmentSize { get; set; }                // "53 ft", "48 ft"

        public decimal? MaxWeight { get; set; }
        public string? WeightUnit { get; set; }                   // "lb", "kg"

        public decimal? RequiredTemperature { get; set; }
        public string? TemperatureUnit { get; set; }              // "F", "C"

        public int Quantity { get; set; }
        public bool IsMandatory { get; set; }
        public bool CopyToLoad { get; set; }

        public string? Notes { get; set; }
    }
}
