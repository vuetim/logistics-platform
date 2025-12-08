using LogisticsPlatform.Domain.Enums;

public class CostLineItemDto
{
    public Guid? Id { get; set; }  // null = new

    public ChargeType Type { get; set; }

    public decimal Qty { get; set; }
    public decimal Price { get; set; }
    public decimal Amount { get; set; }   // optional, for displaying in UI

    public bool IsCustomer { get; set; }
    public bool IsCarrier { get; set; }

    public string? Notes { get; set; }
}
