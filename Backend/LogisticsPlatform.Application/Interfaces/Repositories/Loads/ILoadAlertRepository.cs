using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

public interface ILoadAlertRepository
{
    Task<bool> ExistsAsync(
        Guid loadId,
        Guid? stopId,
        AlertType type,
        AlertSeverity severity);

    Task AddAsync(LoadAlert alert);
}
