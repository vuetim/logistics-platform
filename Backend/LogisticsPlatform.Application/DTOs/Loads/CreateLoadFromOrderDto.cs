namespace LogisticsPlatform.Application.DTOs.Loads
{
    public class CreateLoadFromOrderDto
    {
        public Guid OrderId { get; set; }

        // Optional overrides (execution-level)
        public Guid? CarrierId { get; set; }

        public DateTime? PlannedPickupDate { get; set; }
        public DateTime? PlannedDeliveryDate { get; set; }

        public decimal? CustomerRate { get; set; }
        public decimal? CarrierRate { get; set; }

        // Advanced (future-proof)
        public bool SplitOrder { get; set; } = false;
    }
}
