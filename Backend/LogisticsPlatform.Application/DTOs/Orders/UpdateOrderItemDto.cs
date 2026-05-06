namespace LogisticsPlatform.Application.DTOs.Orders
{
    public class UpdateOrderItemDto
    {
        public string? Name { get; set; }
        public string? CustomerReference { get; set; }

        public decimal? Quantity { get; set; }
        public string? QuantityUnit { get; set; }

        public bool? IsHazmat { get; set; }
        public string? FreightClass { get; set; }
        public string? HazardClass { get; set; }
        public string? IdentificationNumber { get; set; }
        public decimal? ActualQuantity { get; set; }
        public string? Status { get; set; }
        public decimal? HandlingQuantity { get; set; }
        public string? HandlingUnit { get; set; }
        public decimal? UnitNetWeight { get; set; }
        public decimal? UnitGrossWeight { get; set; }
        public string? WeightUnit { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string? DimensionUnit { get; set; }
        public decimal? Volume { get; set; }
        public string? VolumeUnit { get; set; }
        public decimal? MinTemperature { get; set; }
        public decimal? MaxTemperature { get; set; }
        public string? TemperatureUnit { get; set; }
        public decimal? DeclaredValue { get; set; }
        public string? Currency { get; set; }
        public bool? Stackable { get; set; }
        public bool? CopyToLoad { get; set; }
        public string? Notes { get; set; }
    }
}
