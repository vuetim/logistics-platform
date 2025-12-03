using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Domain.Enums;

public class OrderDetailsDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    public string? PreferredCarrierName { get; set; }

    public OrderType OrderType { get; set; }
    public OrderDirection Direction { get; set; }

    public OrderStatus Status { get; set; }
    public OrderPhase Phase { get; set; }

    // Planning window
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DateTime? PlannedPickupDate { get; set; }
    public DateTime? PlannedDeliveryDate { get; set; }

    // Locations
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;

    // Items
    public List<OrderItemDto> Items { get; set; } = new();

    // Audit
    public DateTime CreatedAt { get; set; }
}
