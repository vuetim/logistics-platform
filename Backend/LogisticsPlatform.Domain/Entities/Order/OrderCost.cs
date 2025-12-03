using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class OrderCost : BaseEntity
    {
        //  Aggregate root
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        //  Cost classification
        public OrderCostType CostType { get; set; }   // Freight, Accessorial, Fuel, Other
        public string Code { get; set; } = string.Empty; // e.g. "FRT", "FSC", "DET", "TONU"
        public string Description { get; set; } = string.Empty;

        //  Pricing
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; } = 1;
        public decimal TotalAmount { get; set; }

        //  Billing flags
        public bool IsBillable { get; set; } = true;     // billed to customer
        public bool IsPayable { get; set; } = false;     // payable to carrier
        public bool IsEstimated { get; set; } = false;

        //  Accounting
        public ChargeParty ChargeParty { get; set; }    // Customer / Carrier
        public Currency Currency { get; set; } = Currency.USD;

        //  Notes
        public string? Notes { get; set; }

        //  Copy behavior
        public bool CopyToLoad { get; set; } = true;

        //  Audit snapshot
        public DateTime CostDate { get; set; } = DateTime.UtcNow;
    }
}
