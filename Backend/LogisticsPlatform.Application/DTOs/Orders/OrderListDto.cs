using LogisticsPlatform.Domain.Enums;

public class OrderListDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? PreferredCarrierName { get; set; }

    public OrderStatus Status { get; set; }
    public OrderPhase Phase { get; set; }
    public OrderDirection Direction { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DateTime? PlannedPickupDate { get; set; }
    public DateTime? PlannedDeliveryDate { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public decimal QuotedTotal { get; set; }
    public decimal BaseFreight { get; set; }
    public decimal Accessorials { get; set; }
    public string? Commodity { get; set; }
    public string? PrimaryPONumber { get; set; }
    public string? PrimaryBolNumber { get; set; }
    public string? PrimaryProNumber { get; set; }
    public decimal? TotalWeight { get; set; }
    public int? TotalPallets { get; set; }
    public decimal? TotalVolume { get; set; }
    public bool HasActiveLoad { get; set; }
    public Guid? ActiveLoadId { get; set; }
    public string? ActiveLoadNumber { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
