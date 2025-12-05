using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }
        public Guid? PreferredCarrierId { get; set; }

        public OrderType OrderType { get; set; }
        public OrderDirection Direction { get; set; }

        // Planning window (intent)
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime? PlannedPickupDate { get; set; }
        public DateTime? PlannedDeliveryDate { get; set; }

        //public CreateOrderCostDto? Cost { get; set; }
        //public List<CreateOrderExternalIdDto> ExternalIds { get; set; } = new();
    }
}
