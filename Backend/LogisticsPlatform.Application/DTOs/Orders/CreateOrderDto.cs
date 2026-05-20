using System.ComponentModel.DataAnnotations;
using LogisticsPlatform.Domain.Enums;

public class CreateOrderDto
{
    [Required]
    public Guid CustomerId { get; set; }
    public OrderType OrderType { get; set; }
    public OrderDirection Direction { get; set; }

    [Required]
    public OrderDateDto StartDate { get; set; } = new();

    [Required]
    public OrderDateDto EndDate { get; set; } = new();
    public LookupValueDto? StartDateType { get; set; }
    public LookupValueDto? EndDateType { get; set; }

    public Guid? PreferredCarrierId { get; set; }
    public OrderDateDto? PlannedPickup { get; set; }
    public OrderDateDto? PlannedDelivery { get; set; }

    [MaxLength(80)]
    public string? PrimaryPONumber { get; set; }

    [MaxLength(80)]
    public string? PrimaryBolNumber { get; set; }

    [MaxLength(80)]
    public string? PrimaryProNumber { get; set; }

    [MaxLength(160)]
    public string? Commodity { get; set; }

    [Range(0, 999999999)]
    public decimal? TotalWeight { get; set; }

    [Range(0, 999999)]
    public int? TotalPallets { get; set; }

    [Range(0, 999999999)]
    public decimal? TotalVolume { get; set; }

    [MaxLength(2000)]
    public string? DispatchNotes { get; set; }

    [MaxLength(2000)]
    public string? DeliveryNotes { get; set; }

    [Range(0, 999999999)]
    public decimal? CustomerRate { get; set; }
}
