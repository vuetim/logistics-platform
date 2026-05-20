using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

public class LoadAlertService : ILoadAlertService
{
    private readonly ILoadAlertRepository _repo;
    private readonly ILoadExceptionRepository _exceptions;

    public LoadAlertService(ILoadAlertRepository repo, ILoadExceptionRepository exceptions)
    {
        _repo = repo;
        _exceptions = exceptions;
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

        if (severity is AlertSeverity.Critical or AlertSeverity.Severe)
        {
            var exceptionExists = await _exceptions.ExistsOpenAsync(
                stop.LoadId,
                stop.Id,
                "eta-delay",
                "running-late");

            if (!exceptionExists)
            {
                await _exceptions.AddAsync(new LoadException
                {
                    LoadId = stop.LoadId,
                    LoadStopId = stop.Id,
                    ExceptionKey = "eta-delay",
                    ExceptionValue = "ETA delay",
                    ReasonKey = "running-late",
                    ReasonValue = $"Predicted {minutes} minutes late",
                    ResponsiblePartyKey = "carrier",
                    ResponsiblePartyValue = "Carrier",
                    Status = LoadExceptionStatus.Open,
                    Description = $"Automatic ETA exception for {stop.LocationName} ({stop.StopType}).",
                    OccurredAt = DateTime.UtcNow,
                    CreatedByUserId = Guid.Empty
                });
            }
        }
    }
}
