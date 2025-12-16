using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class OrderEquipmentRequirement : BaseEntity
    {
        public Guid OrderId { get; set; }

        // Planning input
        public string EquipmentType { get; set; } = string.Empty;
        public string? EquipmentSize { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal? MaxWeight { get; set; }
        public WeightUnit WeightUnit { get; set; }

        // Temperature RANGE (jo një vlerë e vetme)
        public decimal? MinTemperature { get; set; }
        public decimal? MaxTemperature { get; set; }
        public TemperatureUnit TemperatureUnit { get; set; }

        // Planning rules
        public bool IsMandatory { get; set; } = true;
        public bool IsPrefered { get; set; } = false;

        public bool CopyToLoad { get; set; } = true;

        public string? Notes { get; set; }
    }

}
