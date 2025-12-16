using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Entities;

public class LoadItem : BaseEntity
{
    public Guid LoadId { get; set; }
    public Load Load { get; set; } = null!;

    public Guid? SourceOrderItemId { get; set; }

    // SNAPSHOT BASIC
    public string Name { get; set; } = null!;
    public string? CustomerReference { get; set; }   // SKU / PO line

    public decimal Quantity { get; set; }
    public string QuantityUnit { get; set; } = null!;
    public bool IsHazmat { get; set; }
    public string? HazardClass { get; set; }

    public string? IdentificationNumber { get; set; } // UN number

    public string? FreightClass { get; set; }

    // EXECUTION DETAILS (qato që po editon)
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

    public bool Stackable { get; set; } = true;
    public string? Notes { get; set; }
    public string? VolumeUnit { get; set; }
    public decimal? Volume { get; set; }
}
