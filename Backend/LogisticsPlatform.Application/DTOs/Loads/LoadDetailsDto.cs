using LogisticsPlatform.Application.DTOs.Costs;
using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Loads.LoadEquipment;

public class LoadDetailsDto
{
    public LoadExecutionDetailsDto Execution { get; set; } = null!;
    public LoadOrderSnapshotDto? OrderSnapshot { get; set; }
    public List<LoadItemDto> Items { get; set; } = new();
    public LoadSummaryDto? Summary { get; set; } = new();
    public LoadCostSummaryDto? CostSummary { get; set; }
    public List<LoadEquipmentDto> Equipment { get; set; } = new();

    public bool HasEquipment { get; set; }




}
