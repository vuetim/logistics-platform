namespace LogisticsPlatform.Application.DTOs.Loads
{
    public class UpdateLoadItemDto
    {
        public decimal? HandlingQuantity { get; set; }
        public string? HandlingUnit { get; set; }

        public decimal? UnitNetWeight { get; set; }
        public decimal? UnitGrossWeight { get; set; }
        public string? WeightUnit { get; set; }

        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string? DimensionUnit { get; set; }

        public decimal? MinTemperature { get; set; }
        public decimal? MaxTemperature { get; set; }
        public string? TemperatureUnit { get; set; }

        public decimal? DeclaredValue { get; set; }
        public string? Currency { get; set; }

        public bool? Stackable { get; set; }
        public string? Notes { get; set; }
    }
}
