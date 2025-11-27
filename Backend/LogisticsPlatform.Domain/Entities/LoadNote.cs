using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class LoadNote : BaseEntity
    {
        public Guid LoadId { get; set; }
        public Load Load { get; set; } = null!;

        public string Message { get; set; } = string.Empty;
        public Guid CreatedByUserId { get; set; }
    }
}
