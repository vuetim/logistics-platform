namespace LogisticsPlatform.Application.DTOs.Carriers.Documents
{
    public class CreateCarrierDocumentDto
    {
        public Guid CarrierId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
    }
}
