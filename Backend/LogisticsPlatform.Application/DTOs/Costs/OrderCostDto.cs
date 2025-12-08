namespace LogisticsPlatform.Application.DTOs.Costs;

public class OrderCostDto
{
    public string? Notes { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal TotalBillable { get; set; }
    public decimal TotalNonBillable { get; set; }

    public List<CostLineItemDto> LineItems { get; set; } = new();
}