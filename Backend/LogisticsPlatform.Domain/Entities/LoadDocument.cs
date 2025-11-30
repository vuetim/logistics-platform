using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class LoadDocument : BaseEntity
    {
        public Guid LoadId { get; set; }
        public Load Load { get; set; } = null!;

        public LoadDocumentType DocumentType { get; set; } = LoadDocumentType.POD;
        public string FileUrl { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
    }
}
