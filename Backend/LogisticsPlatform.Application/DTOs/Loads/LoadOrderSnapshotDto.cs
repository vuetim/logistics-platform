using LogisticsPlatform.Domain.Enums;

public class LoadOrderSnapshotDto
{
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string? PrimaryPONumber { get; set; }
    public string? PrimaryBolNumber { get; set; }
    public string? PrimaryProNumber { get; set; }
    public string? Commodity { get; set; }

    public OrderType OrderType { get; set; }
    public OrderDirection Direction { get; set; }

    public DateTime? PlannedPickupDate { get; set; }
    public DateTime? PlannedDeliveryDate { get; set; }

    public IReadOnlyList<OrderRouteDto> Routes { get; set; } = new List<OrderRouteDto>();
}
