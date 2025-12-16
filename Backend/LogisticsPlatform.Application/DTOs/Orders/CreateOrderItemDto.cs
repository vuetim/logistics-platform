public class CreateOrderItemDto
{
    public string Name { get; set; } = string.Empty;
    public string? CustomerReference { get; set; }

    public decimal Quantity { get; set; }
    public string QuantityUnit { get; set; } = "Pallets";
    public bool IsHazmat { get; set; }
    public string? FreightClass { get; set; }
    public string? HazardClass { get; set; }
    public string? IdentificationNumber { get; set; }

    public string? Notes { get; set; }
}
