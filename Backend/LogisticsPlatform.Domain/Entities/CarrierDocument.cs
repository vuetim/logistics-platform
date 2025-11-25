using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class CarrierDocument : BaseEntity
    {
        public Guid CarrierId { get; set; }
        public Carrier Carrier { get; set; } = null!;

        public string FileName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;

        public DateTime? ExpiresAt { get; set; }

        public Guid UploadedByUserId { get; set; }
        public User UploadedByUser { get; set; } = null!;
    }
}
