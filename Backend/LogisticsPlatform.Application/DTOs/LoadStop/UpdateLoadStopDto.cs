using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.LoadStop
{
    public class UpdateLoadStopDto
    {
        public int Sequence { get; set; }
        public StopType StopType { get; set; }

        public string? LocationName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }

        public DateTime? AppointmentFrom { get; set; }
        public DateTime? AppointmentTo { get; set; }
        public bool HasTime { get; set; }

        public DateTime? PlannedDate { get; set; }
        public string? Notes { get; set; }
    }

}
