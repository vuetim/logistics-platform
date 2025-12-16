using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Domain.Entities
{
    public class LoadDelayResponsibility : BaseEntity
    {
        public Guid LoadId { get; set; }
        public Guid LoadStopId { get; set; }

        public DelayFaultParty FaultParty { get; set; } // Carrier / Shipper / Unknown

        public string Reason { get; set; } = string.Empty;

        public int MinutesLate { get; set; }

        public DateTime DeterminedAt { get; set; } = DateTime.UtcNow;
    }

}
