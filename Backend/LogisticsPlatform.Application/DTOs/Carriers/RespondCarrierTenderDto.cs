using System.ComponentModel.DataAnnotations;

namespace LogisticsPlatform.Application.DTOs.Carriers;

public class RespondCarrierTenderDto
{
    [MaxLength(160)]
    public string? ContactName { get; set; }

    [EmailAddress]
    [MaxLength(200)]
    public string? ContactEmail { get; set; }

    [MaxLength(40)]
    public string? ContactPhone { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
