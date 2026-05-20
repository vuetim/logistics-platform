namespace LogisticsPlatform.Application.DTOs.Search;

public class GlobalSearchResultDto
{
    public string Type { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
}
