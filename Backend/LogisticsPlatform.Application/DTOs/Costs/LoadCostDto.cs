namespace LogisticsPlatform.Application.DTOs.Costs;

public class LoadCostDto
{
    public string? Notes { get; set; }

    public decimal TotalAmount { get; set; }  // total payable

    public List<CostLineItemDto> LineItems { get; set; } = new();
}