using LogisticsPlatform.Domain.Enums;

public class EtaEvaluationResult
{
    public bool IsAtRisk { get; set; }
    public DelayRiskLevel RiskLevel { get; set; }
    public int? MinutesLate { get; set; }
}
