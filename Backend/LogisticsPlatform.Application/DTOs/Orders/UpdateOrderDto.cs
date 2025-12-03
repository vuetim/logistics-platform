namespace LogisticsPlatform.Application.DTOs.Orders
{
    public class UpdateOrderDto
    {
        public DateTime? PlannedPickupDate { get; set; }
        public DateTime? PlannedDeliveryDate { get; set; }

        public Guid? PreferredCarrierId { get; set; }

        //public List<UpdateOrderItemDto>? Items { get; set; }
        //public UpdateOrderCostDto? Cost { get; set; }
    }
}
