using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Domain.Entities.Financial;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Interfaces.Services.Carriers;

public interface ICarrierSettlementService
{
    /// <summary>
    /// Gets the carrier settlement for a load.
    /// If it doesn't exist, auto-creates a draft settlement from load data (Turvo style).
    /// </summary>
    Task<CarrierSettlementDto> GetAsync(Guid loadId);

    /// <summary>
    /// Manual create (nëse ke buton explicit "Create Settlement").
    /// Përndryshe GetAsync mjafton, se bën GetOrCreate.
    /// </summary>
    Task<CarrierSettlementDto> CreateAsync(Guid loadId, CreateSettlementDto dto, Guid userId);

    /// <summary>
    /// Update status (Draft, Sent, Paid, etc).
    /// </summary>
    Task UpdateStatusAsync(Guid settlementId, SettlementStatus status, Guid userId);

    /// <summary>
    /// Returns full entity (për PDF, email, etj.).
    /// </summary>
    Task<CarrierSettlement> GetByIdAsync(Guid settlementId);

    /// <summary>
    /// Persists PDF URL pasi ruhet PDF në storage.
    /// </summary>
    Task UpdatePdfUrlAsync(Guid settlementId, string pdfUrl);
}
