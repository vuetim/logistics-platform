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
        public string? TenderMethod { get; set; }
        public string? TenderNotes { get; set; }
        public DateTime? TenderExpiresAt { get; set; }
        public string? TenderToken { get; set; }
        public DateTime? TenderEmailSentAt { get; set; }
        public string? TenderEmailTo { get; set; }

        // Lifecycle
        public AssignmentStatus Status { get; set; } = AssignmentStatus.Tendered;

        public DateTime TenderedAt { get; set; } = DateTime.UtcNow;
        public DateTime? AcceptedAt { get; set; }
        public string? AcceptedByName { get; set; }
        public string? AcceptedByEmail { get; set; }
        public string? AcceptedByPhone { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string? RejectedReason { get; set; }

        // Flags
        public bool IsActive { get; set; } = true;

        // Audit
        public Guid CreatedByUserId { get; set; }
    }
}
