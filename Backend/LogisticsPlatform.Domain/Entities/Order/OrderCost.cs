using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Entities;

public class OrderCost : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string? Notes { get; set; }

    // Totali i kalkuluar (p.sh. vetëm billable)
    public decimal TotalAmount { get; set; }

    public ICollection<OrderCostLineItem> LineItems { get; set; } = new List<OrderCostLineItem>();
}