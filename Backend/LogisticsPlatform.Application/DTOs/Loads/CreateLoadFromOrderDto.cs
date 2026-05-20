using System.ComponentModel.DataAnnotations;

namespace LogisticsPlatform.Application.DTOs.Loads
{
    public class CreateLoadFromOrderDto
    {
        [Required]
        public Guid OrderId { get; set; }

        public Guid? CarrierId { get; set; }

        public DateTime? PlannedPickupDate { get; set; }
        public DateTime? PlannedDeliveryDate { get; set; }

        [Range(0, 999999999)]
        public decimal? CarrierRate { get; set; }

        [MaxLength(80)]
        public string? RateConfirmationNumber { get; set; }

        public bool SplitOrder { get; set; } = false;
    }
}
