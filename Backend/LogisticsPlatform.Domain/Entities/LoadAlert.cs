using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class LoadAlert : BaseEntity
    {
        public Guid LoadId { get; set; }
        public Load Load { get; set; } = null!;

        public Guid? LoadStopId { get; set; }

        public AlertType Type { get; set; }        // ETA_DELAY
        public AlertSeverity Severity { get; set; } // Warning / Critical / Severe

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime TriggeredAt { get; set; } = DateTime.UtcNow;
    }
}
