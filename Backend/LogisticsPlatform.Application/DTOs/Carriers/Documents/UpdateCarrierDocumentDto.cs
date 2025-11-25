namespace LogisticsPlatform.Application.DTOs.Carriers.Documents
{
    public class UpdateCarrierDocumentDto
    {
        public string? FileName { get; set; }
        public string? DocumentType { get; set; }
        public string? FileUrl { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
