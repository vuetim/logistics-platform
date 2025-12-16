using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

public class DelayFaultAttributionService : IDelayFaultAttributionService
{
    private readonly ILoadDelayResponsibilityRepository _repo;

    public DelayFaultAttributionService(
        ILoadDelayResponsibilityRepository repo)
    {
        _repo = repo;
    }

    public async Task EvaluateAsync(LoadStop stop)
    {
        if (!stop.IsAtRiskOfDelay || stop.MinutesLatePrediction is null)
            return;

        DelayFaultParty party;
        string reason;

        // =========================
        // TURVO-STYLE RULES
        // =========================

        // 1️⃣ Shipper fault (appointment moved / revised)
        if (stop.RevisedArrivalTo.HasValue &&
            stop.PlannedArrivalTo.HasValue &&
            stop.RevisedArrivalTo > stop.PlannedArrivalTo)
        {
            party = DelayFaultParty.Shipper;
            reason = "Appointment revised by shipper";
        }
        // 2️⃣ Carrier late to pickup
        else if (stop.StopType == StopType.Pickup)
        {
            party = DelayFaultParty.Carrier;
            reason = "Carrier late to pickup";
        }
        // 3️⃣ Carrier late to delivery
        else if (stop.StopType == StopType.Delivery)
        {
            party = DelayFaultParty.Carrier;
            reason = "Carrier late to delivery";
        }
        else
        {
            party = DelayFaultParty.Unknown;
            reason = "Unable to determine delay responsibility";
        }

        var entity = new LoadDelayResponsibility
        {
            LoadId = stop.LoadId,
            LoadStopId = stop.Id,
            FaultParty = party,
            MinutesLate = stop.MinutesLatePrediction.Value,
            Reason = reason
        };

        await _repo.AddAsync(entity);
    }
}
