using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.LoadStop
{
    public class LoadStopDto
    {
        public Guid Id { get; set; }

        public StopType StopType { get; set; }
        public int Sequence { get; set; }

        public string LocationName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;

        public DateTime? PlannedDate { get; set; }

        public DateTime? AppointmentFrom { get; set; }
        public DateTime? AppointmentTo { get; set; }
        public bool HasTime { get; set; }

        public string? Notes { get; set; }
    }
}
