using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class OrderExternalId : BaseEntity
    {
        // =========================
        // Relations
        // =========================

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // =========================
        // External Reference
        // =========================

        // e.g. "PO", "BOL", "PRO", "Truck ID", "Reference #"
        public string Type { get; set; } = string.Empty;

        // Actual value
        public string Value { get; set; } = string.Empty;

        // =========================
        // Optional ownership
        // =========================

        // Customer / Carrier / Internal
        public string? RelatedParty { get; set; }

        // Copy behavior
        public bool CopyToLoad { get; set; } = true;
    }
}
