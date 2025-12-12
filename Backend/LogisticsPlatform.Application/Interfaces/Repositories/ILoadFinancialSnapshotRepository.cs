using LogisticsPlatform.Application.DTOs.Costs;

public interface ILoadFinancialSnapshotRepository
{
    Task<LoadCostSummaryDto> GetSnapshotAsync(Guid loadId);
}
