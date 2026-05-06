namespace LogisticsPlatform.Application.DTOs.Orders.ExternalIds
{
    public class CreateOrderExternalIdDto
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? RelatedParty { get; set; }
        public bool CopyToLoad { get; set; } = true;
    }
}
