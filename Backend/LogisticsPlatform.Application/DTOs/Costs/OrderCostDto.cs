namespace LogisticsPlatform.Application.DTOs.Costs;

public class OrderCostDto
{
    public string? BillTo { get; set; }
    public string? Notes { get; set; }
    public decimal TaxRate { get; set; }

    public decimal BaseFreight { get; set; }
    public decimal Accessorials { get; set; }
    public decimal QuotedTotal { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TotalTax { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal TotalBillable { get; set; }
    public decimal TotalNonBillable { get; set; }

    public List<CostLineItemDto> LineItems { get; set; } = new();
}
