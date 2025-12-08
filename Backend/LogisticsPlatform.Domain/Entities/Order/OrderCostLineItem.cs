using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities;

public class OrderCostLineItem : BaseEntity
{
    public Guid OrderCostId { get; set; }
    public OrderCost OrderCost { get; set; } = null!;

    public string TypeCode { get; set; } = null!;   //  "FREIGHT_FLAT"
    public string TypeLabel { get; set; } = null!;  //  "Freight - flat"

    public decimal Qty { get; set; }
    public decimal Price { get; set; }
    public decimal Amount { get; set; }             // Qty * Price

    public bool Billable { get; set; } = true;      // for customer
    public string? Notes { get; set; }

    public bool IsCustomer { get; set; } = true;   // billable charges
    public bool IsCarrier { get; set; } = false;   // never carrier cost here
    public ChargeType Type { get; set; }

}