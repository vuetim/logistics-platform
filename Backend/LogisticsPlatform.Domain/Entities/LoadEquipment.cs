using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class LoadEquipment : BaseEntity
    {
        public Guid LoadId { get; set; }
        public Load Load { get; set; } = null!;

        public EquipmentType EquipmentType { get; set; }
        public decimal? Length { get; set; }

        public decimal? Weight { get; set; }
        public WeightUnit WeightUnit { get; set; } = WeightUnit.Lb;

        // Reefer
        public decimal? MinTemp { get; set; }
        public decimal? MaxTemp { get; set; }
        public TemperatureUnit TempUnit { get; set; } = TemperatureUnit.F;
    }
}
