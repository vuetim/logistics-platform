using LogisticsPlatform.Domain.Enums;

public class OrderListDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? PreferredCarrierName { get; set; }

    public OrderStatus Status { get; set; }
    public OrderPhase Phase { get; set; }
    public DateTime CreatedAt { get; set; }
}
