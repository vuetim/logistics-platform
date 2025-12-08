using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities;

public class LoadCostLineItem : BaseEntity
{
    public Guid LoadCostId { get; set; }
    public LoadCost LoadCost { get; set; } = null!;

    public string TypeCode { get; set; } = null!;
    public string TypeLabel { get; set; } = null!;

    public decimal Qty { get; set; }
    public decimal Price { get; set; }
    public decimal Amount { get; set; }

    public bool Payable { get; set; } = true;  // for carrier
    public string? Notes { get; set; }
    public bool IsCustomer { get; set; } // billable
    public bool IsCarrier { get; set; }  // payable
    public ChargeType Type { get; set; }
}