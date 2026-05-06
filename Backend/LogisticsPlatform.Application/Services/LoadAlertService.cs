using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

public class LoadAlertService : ILoadAlertService
{
    private readonly ILoadAlertRepository _repo;

    public LoadAlertService(ILoadAlertRepository repo)
    {
        _repo = repo;
    }

    public async Task HandleEtaDelayAsync(LoadStop stop)
    {
        if (!stop.IsAtRiskOfDelay || stop.MinutesLatePrediction is null)
            return;

        var minutes = stop.MinutesLatePrediction.Value;

        AlertSeverity? severity = minutes switch
        {
            >= 90 => AlertSeverity.Severe,
            >= 60 => AlertSeverity.Critical,
            >= 30 => AlertSeverity.Warning,
            _ => null
        };

        if (severity is null)
            return;

        var exists = await _repo.ExistsAsync(
            stop.LoadId,
            stop.Id,
            AlertType.EtaDelay,
            severity.Value);

        if (exists)
            return; //  no spam 

        var alert = new LoadAlert
        {
            LoadId = stop.LoadId,
            LoadStopId = stop.Id,
            Type = AlertType.EtaDelay,
            Severity = severity.Value,
            Message =
                $"Predicted {minutes} min late at {stop.LocationName} ({stop.StopType})"
        };

        await _repo.AddAsync(alert);
    }
}
