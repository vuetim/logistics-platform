using LogisticsPlatform.Domain.Enums;

public class LoadOrderSnapshotDto
{
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    public OrderType OrderType { get; set; }
    public OrderDirection Direction { get; set; }

    public DateTime? PlannedPickupDate { get; set; }
    public DateTime? PlannedDeliveryDate { get; set; }

    public IReadOnlyList<OrderRouteDto> Routes { get; set; } = new List<OrderRouteDto>();
}
