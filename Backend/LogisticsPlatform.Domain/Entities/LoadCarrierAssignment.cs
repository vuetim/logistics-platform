using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class LoadCarrierAssignment : BaseEntity
    {
        public Guid LoadId { get; set; }
        public Load Load { get; set; } = null!;

        public Guid CarrierId { get; set; }
        public Carrier Carrier { get; set; } = null!;

        // Commercial
        public decimal? OfferedRate { get; set; }
        public string? Currency { get; set; }

        public string? RateConfirmationNumber { get; set; }

        // Lifecycle
        public AssignmentStatus Status { get; set; } = AssignmentStatus.Tendered;

        public DateTime TenderedAt { get; set; } = DateTime.UtcNow;
        public DateTime? AcceptedAt { get; set; }
        public DateTime? RejectedAt { get; set; }

        // Flags
        public bool IsActive { get; set; } = true;

        // Audit
        public Guid CreatedByUserId { get; set; }
    }
}
