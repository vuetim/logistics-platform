using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class LoadEquipment : BaseEntity
    {
        public Guid LoadId { get; set; }
        public Load Load { get; set; } = null!;

        // Traceability
        public Guid? SourceOrderEquipmentRequirementId { get; set; }

        // What is actually used
        public EquipmentType EquipmentType { get; set; }

        public int Quantity { get; set; } = 1;

        // Physical constraints
        public decimal? Length { get; set; }

        public decimal? Weight { get; set; }
        public WeightUnit WeightUnit { get; set; } = WeightUnit.Lb;

        // Reefer execution
        public decimal? MinTemp { get; set; }
        public decimal? MaxTemp { get; set; }
        public TemperatureUnit TempUnit { get; set; } = TemperatureUnit.F;
        public bool IsPrefered { get; set; }
    }
}

