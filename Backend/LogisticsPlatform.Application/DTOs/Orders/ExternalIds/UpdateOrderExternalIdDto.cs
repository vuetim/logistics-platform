namespace LogisticsPlatform.Application.DTOs.Orders.ExternalIds
{
    public class UpdateOrderExternalIdDto
    {
        public string? Type { get; set; }
        public string? Value { get; set; }
        public string? RelatedParty { get; set; }
        public bool? CopyToLoad { get; set; }
    }
}
