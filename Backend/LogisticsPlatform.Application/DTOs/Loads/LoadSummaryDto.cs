using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Loads
{
    public class LoadSummaryDto
    {
        public decimal TotalWeight { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal TotalPallets { get; set; }
        public int TotalItems { get; set; }
        public int TotalStops { get; set; }
        public int PickupStops { get; set; }
        public int DeliveryStops { get; set; }
        public List<string> PickupLocations { get; set; } = new();
        public List<string> DeliveryLocations { get; set; } = new();
    }
}
