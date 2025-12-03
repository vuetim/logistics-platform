using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class OrderEquipmentRequirement : BaseEntity
    {
        // =========================
        // Relations
        // =========================

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // =========================
        // Equipment Requirement
        // =========================

        // e.g. "Dry Van", "Reefer", "Flatbed"
        public string EquipmentType { get; set; } = string.Empty;

        // e.g. "53 ft", "48 ft"
        public string? EquipmentSize { get; set; }

        // =========================
        // Constraints
        // =========================

        // Weight capacity hint (lbs / kg – snapshot)
        public decimal? MaxWeight { get; set; }
        public string? WeightUnit { get; set; }   // lb / kg

        // Temperature requirement (for reefer)
        public decimal? RequiredTemperature { get; set; }
        public string? TemperatureUnit { get; set; } // F / C

        // =========================
        // Quantity
        // =========================

        // e.g. need 2 trailers
        public int Quantity { get; set; } = 1;

        // =========================
        // Flags
        // =========================

        public bool IsMandatory { get; set; } = true;
        public bool CopyToLoad { get; set; } = true;

        // =========================
        // Notes
        // =========================

        public string? Notes { get; set; }
    }
}
