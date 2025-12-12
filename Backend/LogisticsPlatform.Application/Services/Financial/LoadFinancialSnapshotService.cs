using LogisticsPlatform.Application.DTOs.Costs;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;

namespace LogisticsPlatform.Application.Services.Financial;

public class LoadFinancialSnapshotService : ILoadFinancialSnapshotService
{
    private readonly ILoadFinancialSnapshotRepository _repo;

    public LoadFinancialSnapshotService(ILoadFinancialSnapshotRepository repo)
    {
        _repo = repo;
    }

    public Task<LoadCostSummaryDto> GetSnapshotAsync(Guid loadId)
    {
        return _repo.GetSnapshotAsync(loadId);
    }
}
