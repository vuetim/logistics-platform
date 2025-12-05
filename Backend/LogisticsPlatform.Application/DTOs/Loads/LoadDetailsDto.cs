using LogisticsPlatform.Application.DTOs.Loads;

public class LoadDetailsDto
{
    public LoadExecutionDetailsDto Execution { get; set; } = null!;
    public LoadOrderSnapshotDto? OrderSnapshot { get; set; }
    public List<LoadItemDto> Items { get; set; } = new();
    public LoadSummaryDto Summary { get; set; } = new();




}
