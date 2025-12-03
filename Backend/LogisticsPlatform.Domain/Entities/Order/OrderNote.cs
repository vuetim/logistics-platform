using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class OrderNote : BaseEntity
    {
        // =========================
        // Relations
        // =========================

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // =========================
        // Note Info
        // =========================

        public string Message { get; set; } = string.Empty;

        // Internal vs Customer-visible
        public bool IsInternal { get; set; } = false;

        // =========================
        // Audit
        // =========================

        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
    }
}
