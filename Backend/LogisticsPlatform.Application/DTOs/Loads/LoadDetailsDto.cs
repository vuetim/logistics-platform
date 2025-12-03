public class LoadDetailsDto
{
    public LoadExecutionDetailsDto Execution { get; set; } = null!;
    public LoadOrderSnapshotDto? OrderSnapshot { get; set; }
}
