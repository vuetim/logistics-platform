using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class LoadOrder : BaseEntity
    {
        public Guid LoadId { get; set; }
        public Load Load { get; set; } = null!;

        public string PONumber { get; set; } = string.Empty;
    }
}
