using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;

        // =========================
        // Relations
        // =========================

        // REQUIRED – kush po e porosit
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        // OPTIONAL – carrier i kontraktuar paraprakisht (jo execution)
        public Guid? PreferredCarrierId { get; set; }
        public Carrier? PreferredCarrier { get; set; }

        // =========================
        // Classification
        // =========================

        public OrderType OrderType { get; set; } = OrderType.Transportation;
        public OrderDirection Direction { get; set; } = OrderDirection.Outbound;

        public OrderStatus Status { get; set; } = OrderStatus.Draft;
        public OrderPhase Phase { get; set; } = OrderPhase.Open;

        // =========================
        // Planning Window (INTENT)
        // =========================

        public DateTime StartDate { get; set; }       // earliest pickup allowed
        public DateTime EndDate { get; set; }         // latest delivery allowed

        public DateTime? PlannedPickupDate { get; set; }
        public DateTime? PlannedDeliveryDate { get; set; }

        // =========================
        // Origin & Destination (INTENT)
        // =========================

        //public Guid OriginAddressId { get; set; }
        //public CustomerAddress OriginAddress { get; set; } = null!;

        //public Guid DestinationAddressId { get; set; }
        //public CustomerAddress DestinationAddress { get; set; } = null!;
        public string? PrimaryPONumber { get; set; }
        public string? PrimaryBolNumber { get; set; }
        public string? PrimaryProNumber { get; set; }

        public string? Commodity { get; set; }
        public decimal? TotalWeight { get; set; }
        public int? TotalPallets { get; set; }
        public decimal? TotalVolume { get; set; }

        public string? DispatchNotes { get; set; }
        public string? DeliveryNotes { get; set; }


        // =========================
        // Child aggregates
        // =========================



        // WHAT is being shipped
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        // WHAT equipment is required
        public ICollection<OrderEquipmentRequirement> EquipmentRequirements { get; set; }
            = new List<OrderEquipmentRequirement>();

        // Customer charges
        public OrderCost? Cost { get; set; }
        public decimal? CustomerRate { get; set; }

        // External customer references (PO, Ref #)
        public ICollection<OrderExternalId> ExternalIds { get; set; }
            = new List<OrderExternalId>();

        // Notes & documents (commercial)
        public ICollection<OrderNote> Notes { get; set; } = new List<OrderNote>();
        public ICollection<OrderDocument> Documents { get; set; } = new List<OrderDocument>();

        // =========================
        // Link to execution (Loads)
        // =========================
        public ICollection<OrderRoute> OrderRoutes { get; set; } = new List<OrderRoute>();

        public ICollection<LoadOrder> Loads { get; set; } = new List<LoadOrder>();

        // =========================
        // Audit
        // =========================

        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
    }
}
