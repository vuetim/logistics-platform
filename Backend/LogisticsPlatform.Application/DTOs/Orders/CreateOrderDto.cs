using LogisticsPlatform.Domain.Enums;

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    public OrderType OrderType { get; set; }
    public OrderDirection Direction { get; set; }
    public OrderDateDto StartDate { get; set; } = new();
    public OrderDateDto EndDate { get; set; } = new();
    public LookupValueDto? StartDateType { get; set; }
    public LookupValueDto? EndDateType { get; set; }

    public Guid? PreferredCarrierId { get; set; }
    public OrderDateDto? PlannedPickup { get; set; }
    public OrderDateDto? PlannedDelivery { get; set; }

    // Optional business fields
    public string? PrimaryPONumber { get; set; }
    public string? PrimaryBolNumber { get; set; }
    public string? PrimaryProNumber { get; set; }

    public string? Commodity { get; set; }
    public decimal? TotalWeight { get; set; }
    public int? TotalPallets { get; set; }
    public decimal? TotalVolume { get; set; }

    public string? DispatchNotes { get; set; }
    public string? DeliveryNotes { get; set; }
    public decimal? CustomerRate { get; set; }
}
