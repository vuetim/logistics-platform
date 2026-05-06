using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Entities;

public class OrderCost : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string? BillTo { get; set; }
    public string? Notes { get; set; }
    public decimal TaxRate { get; set; } // percent (0-100)

    // Totali i kalkuluar (p.sh. vetëm billable)
    public decimal TotalAmount { get; set; }

    public ICollection<OrderCostLineItem> LineItems { get; set; } = new List<OrderCostLineItem>();
    public decimal QuotedTotal { get; set; }
    public decimal Accessorials { get; set; }
}
