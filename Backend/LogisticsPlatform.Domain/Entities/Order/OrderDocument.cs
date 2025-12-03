using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class OrderDocument : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public OrderDocumentType DocumentType { get; set; } = OrderDocumentType.PO;
        public string FileUrl { get; set; } = string.Empty;

        // Internal (ops / accounting) vs external (customer)
        public bool IsInternal { get; set; }

        // When creating Load
        public bool CopyToLoad { get; set; } = true;
    }
}
