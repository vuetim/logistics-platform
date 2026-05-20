using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Loads.Exceptions;

public class UpdateLoadExceptionRequest
{
    public string? ExceptionKey { get; set; }
    public string? ExceptionValue { get; set; }
    public string? ReasonKey { get; set; }
    public string? ReasonValue { get; set; }
    public string? EdiReasonCode { get; set; }
    public string? ResponsiblePartyKey { get; set; }
    public string? ResponsiblePartyValue { get; set; }
    public LoadExceptionStatus? Status { get; set; }
    public string? Description { get; set; }
    public string? ResolutionNotes { get; set; }
}
