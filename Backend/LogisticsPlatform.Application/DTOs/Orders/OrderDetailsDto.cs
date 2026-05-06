using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Domain.Enums;

public class OrderDetailsDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    public Guid? PreferredCarrierId { get; set; }
    public string? PreferredCarrierName { get; set; }

    public OrderType OrderType { get; set; }
    public OrderDirection Direction { get; set; }

    public OrderStatus Status { get; set; }
    public OrderPhase Phase { get; set; }

    // Planning window
    public OrderDateDto StartDate { get; set; } = new();
    public OrderDateDto EndDate { get; set; } = new();
    public LookupValueDto? StartDateType { get; set; }
    public LookupValueDto? EndDateType { get; set; }

    public OrderDateDto? PlannedPickup { get; set; }
    public OrderDateDto? PlannedDelivery { get; set; }

    // Locations
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;

    public string? DispatchNotes { get; set; }
    public string? DeliveryNotes { get; set; }
    public decimal? CustomerRate { get; set; }
    public decimal BaseFreight { get; set; }
    public decimal Accessorials { get; set; }
    public decimal QuotedTotal { get; set; }

    public string? PrimaryPONumber { get; set; }
    public string? PrimaryBolNumber { get; set; }
    public string? PrimaryProNumber { get; set; }
    public string? Commodity { get; set; }
    public decimal? TotalWeight { get; set; }
    public int? TotalPallets { get; set; }
    public decimal? TotalVolume { get; set; }
    public bool HasActiveLoad { get; set; }
    public Guid? ActiveLoadId { get; set; }
    public string? ActiveLoadNumber { get; set; }

    // Items
    public List<OrderItemDto> Items { get; set; } = new();

    // Audit
    public DateTime CreatedAt { get; set; }
}
