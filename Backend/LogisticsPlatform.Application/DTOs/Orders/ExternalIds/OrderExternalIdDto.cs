namespace LogisticsPlatform.Application.DTOs.Orders.ExternalIds
{
    public class OrderExternalIdDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? RelatedParty { get; set; }
        public bool CopyToLoad { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
