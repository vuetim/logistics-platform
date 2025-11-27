using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Loads.LoadEquipment
{
    public class CreateLoadEquipmentDto
    {
        public EquipmentType EquipmentType { get; set; }

        public decimal? Length { get; set; }

        public decimal? Weight { get; set; }
        public WeightUnit WeightUnit { get; set; } = WeightUnit.Lb;

        // Reefer only
        public decimal? MinTemp { get; set; }
        public decimal? MaxTemp { get; set; }
        public TemperatureUnit TempUnit { get; set; } = TemperatureUnit.F;
    }
}
