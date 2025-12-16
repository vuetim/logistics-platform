using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services
{
    public class EtaPredictionService : IEtaPredictionService
    {
        public Task<EtaEvaluationResult?> EvaluateAsync(LoadStop stop)
        {
            if (!stop.PlannedArrivalTo.HasValue || !stop.PredictedArrivalAt.HasValue)
                return Task.FromResult<EtaEvaluationResult?>(null);

            var minutesLate =
                (int)(stop.PredictedArrivalAt.Value - stop.PlannedArrivalTo.Value).TotalMinutes;

            if (minutesLate <= 0)
            {
                stop.DelayRisk = DelayRiskLevel.None;
                stop.IsAtRiskOfDelay = false;
                stop.MinutesLatePrediction = null;
                return Task.FromResult<EtaEvaluationResult?>(null);
            }

            stop.IsAtRiskOfDelay = true;
            stop.MinutesLatePrediction = minutesLate;

            var risk = minutesLate switch
            {
                >= 90 => DelayRiskLevel.Severe90,
                >= 60 => DelayRiskLevel.Critical60,
                >= 30 => DelayRiskLevel.Warning30,
                _ => DelayRiskLevel.None
            };

            stop.DelayRisk = risk;

            return Task.FromResult<EtaEvaluationResult?>(new EtaEvaluationResult
            {
                IsAtRisk = true,
                RiskLevel = risk,
                MinutesLate = minutesLate
            });
        }
    }
}
