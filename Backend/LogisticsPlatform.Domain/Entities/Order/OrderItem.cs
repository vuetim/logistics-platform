using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        // =========================
        // Relations
        // =========================

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // =========================
        // Identification
        // =========================

        // e.g. "Frozen Beef", "Electronics", "Paper Rolls"
        public string Name { get; set; } = string.Empty;

        // Optional classification
        public string? Category { get; set; }

        // External / customer references
        public string? CustomerReference { get; set; }   // PO line, SKU, etc.
        public string? LotNumber { get; set; }

        // =========================
        // Quantity & Handling
        // =========================

        // e.g. 20 Pallets, 100 Boxes
        public decimal Quantity { get; set; }
        public string QuantityUnit { get; set; } = "Pallets";

        // optional, for handling hints
        public decimal? HandlingQuantity { get; set; }
        public string? HandlingUnit { get; set; }

        // =========================
        // Weight
        // =========================

        public decimal? UnitNetWeight { get; set; }
        public decimal? UnitGrossWeight { get; set; }
        public string? WeightUnit { get; set; }           // lb, kg

        // =========================
        // Dimensions
        // =========================

        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string? DimensionUnit { get; set; }        // in, cm, m

        // =========================
        // Volume (optional but useful)
        // =========================

        public decimal? Volume { get; set; }
        public string? VolumeUnit { get; set; }           // cbm, cuft

        // =========================
        // Temperature / Reefer
        // =========================

        public decimal? MinTemperature { get; set; }
        public decimal? MaxTemperature { get; set; }
        public string? TemperatureUnit { get; set; }      // F, C

        // =========================
        // Hazmat / Freight
        // =========================

        public bool IsHazmat { get; set; } = false;
        public string? HazardClass { get; set; }
        public string? ShippingName { get; set; }
        public string? IdentificationNumber { get; set; } // UN number

        public string? FreightClass { get; set; }          // e.g. 70, 77.5
        public string? NmfcCode { get; set; }
        public string? NmfcSubCode { get; set; }
        public decimal? DeclaredValue { get; set; }
        public string? Currency { get; set; }

        // =========================
        // Loadability
        // =========================
        public bool CopyToLoad { get; set; } = true;

        public bool Stackable { get; set; } = true;
        public string? Notes { get; set; }
    }
}
