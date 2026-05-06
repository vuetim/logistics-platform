using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Orders.Documents
{
    public class OrderDocumentDto
    {
        public Guid Id { get; set; }
        public OrderDocumentType DocumentType { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
        public bool CopyToLoad { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
