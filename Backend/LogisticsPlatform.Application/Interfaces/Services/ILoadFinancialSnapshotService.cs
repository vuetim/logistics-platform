using LogisticsPlatform.Application.DTOs.Costs;

namespace LogisticsPlatform.Application.Interfaces.Services;

public interface ILoadFinancialSnapshotService
{
    Task<LoadCostSummaryDto> GetSnapshotAsync(Guid loadId);
}
