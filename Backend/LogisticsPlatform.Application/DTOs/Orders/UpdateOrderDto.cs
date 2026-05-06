using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Orders
{
    public class UpdateOrderDto
    {
        public OrderType? OrderType { get; set; }
        public OrderDirection? Direction { get; set; }
        public OrderDateDto? StartDate { get; set; }
        public OrderDateDto? EndDate { get; set; }
        public LookupValueDto? StartDateType { get; set; }
        public LookupValueDto? EndDateType { get; set; }

        public OrderDateDto? PlannedPickup { get; set; }
        public OrderDateDto? PlannedDelivery { get; set; }

        public string? DispatchNotes { get; set; }
        public string? DeliveryNotes { get; set; }

        public Guid? PreferredCarrierId { get; set; }

        public string? PrimaryPONumber { get; set; }
        public string? PrimaryBolNumber { get; set; }
        public string? PrimaryProNumber { get; set; }

        public string? Commodity { get; set; }
        public decimal? TotalWeight { get; set; }
        public int? TotalPallets { get; set; }
        public decimal? TotalVolume { get; set; }

        public decimal? CustomerRate { get; set; }
    }
}
