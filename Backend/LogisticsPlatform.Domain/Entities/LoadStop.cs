using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class LoadStop : BaseEntity
    {
        public Guid LoadId { get; set; }
        public Load Load { get; set; } = null!;

        public int Sequence { get; set; }
        public StopType StopType { get; set; }

        public string LocationName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;

        // Appointment window
        public DateTime? AppointmentFrom { get; set; }
        public DateTime? AppointmentTo { get; set; }
        public bool HasTime { get; set; }

        public string? Notes { get; set; }
        public DateTime? PlannedDate { get; set; }


    }
}
