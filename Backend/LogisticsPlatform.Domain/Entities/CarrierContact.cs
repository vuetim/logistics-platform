using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class CarrierContact : BaseEntity
    {
        public Guid CarrierId { get; set; }
        public Carrier Carrier { get; set; } = null!;

        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }
    }
}
