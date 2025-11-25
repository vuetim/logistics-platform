using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class CarrierNote : BaseEntity
    {
        public Guid CarrierId { get; set; }
        public Carrier Carrier { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
    }
}
