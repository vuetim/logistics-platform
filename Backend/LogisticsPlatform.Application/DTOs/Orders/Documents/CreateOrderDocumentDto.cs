using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Orders.Documents
{
    public class CreateOrderDocumentDto
    {
        public OrderDocumentType DocumentType { get; set; } = OrderDocumentType.Other;
        public string FileUrl { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
        public bool CopyToLoad { get; set; } = true;
    }
}
