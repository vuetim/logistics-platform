using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities;

public class LoadException : BaseEntity
{
    public Guid LoadId { get; set; }
    public Load Load { get; set; } = null!;

    public Guid? LoadStopId { get; set; }
    public LoadStop? LoadStop { get; set; }

    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }

    public string ExceptionKey { get; set; } = string.Empty;
    public string ExceptionValue { get; set; } = string.Empty;
    public string? ReasonKey { get; set; }
    public string? ReasonValue { get; set; }
    public string? EdiReasonCode { get; set; }
    public string? ResponsiblePartyKey { get; set; }
    public string? ResponsiblePartyValue { get; set; }
    public LoadExceptionStatus Status { get; set; } = LoadExceptionStatus.Open;
    public string? Description { get; set; }
    public string? ResolutionNotes { get; set; }
    public string? AffectedItemName { get; set; }
    public string? AffectedItemReference { get; set; }
    public decimal? Quantity { get; set; }
    public string? Unit { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
    public Guid CreatedByUserId { get; set; }
}
