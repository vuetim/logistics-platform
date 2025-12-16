using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Carriers
{
    public class CarrierScorecCardDto
    {
        public Guid CarrierId { get; set; }
        public int TotalStops { get; set; }
        public int OnTimeStops { get; set; }
        public double OnTimePercentage { get; set; }
        public int AvgMinutesLate { get; set; }
    }

}
