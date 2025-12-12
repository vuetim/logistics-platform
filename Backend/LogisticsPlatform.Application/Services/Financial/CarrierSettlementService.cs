using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Entities.Financial;
using LogisticsPlatform.Domain.Enums;
using SendGrid.Helpers.Errors.Model;

namespace LogisticsPlatform.Application.Services.Financial;

public class CarrierSettlementService : ICarrierSettlementService
{
    private readonly ICarrierSettlementRepository _repo;
    private readonly ILoadRepository _loads;

    public CarrierSettlementService(
        ICarrierSettlementRepository repo,
        ILoadRepository loads)
    {
        _repo = repo;
        _loads = loads;
    }

    /// <summary>
    /// Turvo-style:
    /// - Nëse ekziston settlement për këtë load → ktheje
    /// - Nëse nuk ekziston → auto-create draft settlement nga load data
    /// </summary>
    public async Task<CarrierSettlementDto> GetAsync(Guid loadId)
    {
        // 1) Provo me gjet ekzistues
        var existing = await _repo.GetByLoadIdAsync(loadId);
        if (existing != null)
        {
            // Nëse invoice është Draft → rifreskoje nga load
            if (existing.Status == SettlementStatus.Draft)
            {
                var loadForRefresh = await _loads.GetByIdAsync(loadId)
                    ?? throw new NotFoundException("Load not found.");

                RecalculateDraftSettlement(existing, loadForRefresh);
                await _repo.SaveChangesAsync();
            }

            return Map(existing);
        }

        // 2) Nuk ekziston → auto-create
        var load = await _loads.GetByIdAsync(loadId)
            ?? throw new NotFoundException("Load not found.");

        if (load.CarrierId == null)
            throw new BusinessValidationException("Cannot generate carrier settlement: load has no assigned carrier.");

        var dto = new CreateSettlementDto
        {
            SettlementDate = DateTime.UtcNow,
            Notes = "Auto-created draft settlement."
        };

        var created = await CreateInternalAsync(load, dto, load.CreatedByUserId, isAuto: true);

        return Map(created);
    }

    /// <summary>
    /// Manual create – p.sh. kur ke buton "Create Settlement".
    /// </summary>
    public async Task<CarrierSettlementDto> CreateAsync(
        Guid loadId,
        CreateSettlementDto dto,
        Guid userId)
    {
        var existing = await _repo.GetByLoadIdAsync(loadId);
        if (existing != null)
            throw new BusinessValidationException("A settlement already exists for this load.");

        var load = await _loads.GetByIdAsync(loadId)
            ?? throw new NotFoundException("Load not found.");

        if (load.CarrierId == null)
            throw new BusinessValidationException("Cannot create carrier settlement: load has no assigned carrier.");

        var settlement = await CreateInternalAsync(load, dto, userId, isAuto: false);

        return Map(settlement);
    }

    private async Task<CarrierSettlement> CreateInternalAsync(
        Load load,
        CreateSettlementDto dto,
        Guid userId,
        bool isAuto)
    {
        // 1) Build line items from load (CarrierRate + carrier accessorials)
        var lineItems = BuildLineItemsFromLoad(load);

        if (!lineItems.Any())
        {
            // opcion: ose lejo 0, ose mos e lejo
            throw new BusinessValidationException("Cannot create carrier settlement: no payable amounts found for this load.");
        }

        var total = lineItems.Sum(x => x.Amount);

        var settlement = new CarrierSettlement
        {
            LoadId = load.Id,
            Load = load,
            CarrierId = load.CarrierId!.Value,
            Carrier = load.Carrier!,
            SettlementNumber = $"SET-{DateTime.UtcNow:yyyyMMddHHmmss}",
            SettlementDate = dto.SettlementDate,
            Status = SettlementStatus.Draft,
            Notes = dto.Notes,
            TotalAmount = total,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = userId
        };
        foreach (var li in lineItems)
        {
            li.Settlement = settlement;
        }

        settlement.LineItems = lineItems;
        var carrierTerms = load.Carrier.PaymentTermsDays;
        settlement.DueDate = dto.SettlementDate.AddDays(carrierTerms);
        await _repo.AddAsync(settlement);
        await _repo.SaveChangesAsync();

        return settlement;
    }

    /// <summary>
    /// Carrier payable lines:
    /// - Freight line nga CarrierRate
    /// - Accessorials nga Load.Cost.LineItems ku IsCarrier == true
    /// </summary>
    private static List<CarrierSettlementLineItem> BuildLineItemsFromLoad(Load load)
    {
        var result = new List<CarrierSettlementLineItem>();

        // 1) Freight / Linehaul nga CarrierRate
        var carrierRate = load.CarrierRate ?? 0;
        if (carrierRate > 0)
        {
            result.Add(new CarrierSettlementLineItem
            {
                Description = "Linehaul",
                Qty = 1,
                Price = carrierRate,
                Amount = carrierRate,
                Billable = true
            });
        }

        // 2) Accessorials për carrier (IsCarrier = true)
        if (load.Cost?.LineItems != null)
        {
            var extras = load.Cost.LineItems
                .Where(x => x.IsCarrier)
                .Select(x => new CarrierSettlementLineItem
                {
                    Description = x.Notes ?? x.Type.ToString(),
                    Qty = x.Qty,
                    Price = x.Price,
                    Amount = x.Qty * x.Price,
                    Billable = true,
                    Notes = x.Notes
                });

            result.AddRange(extras);
        }

        return result;
    }

    public async Task<CarrierSettlement> GetByIdAsync(Guid settlementId)
    {
        var settlement = await _repo.GetByIdAsync(settlementId);
        if (settlement == null)
            throw new NotFoundException("Settlement not found.");

        return settlement;
    }

    public async Task UpdateStatusAsync(Guid settlementId, SettlementStatus status, Guid userId)
    {
        var settlement = await _repo.GetByIdAsync(settlementId)
            ?? throw new NotFoundException("Settlement not found.");

        settlement.Status = status;
        settlement.UpdatedAt = DateTime.UtcNow;
        settlement.UpdatedByUserId = userId;

        await _repo.SaveChangesAsync();
    }

    public async Task UpdatePdfUrlAsync(Guid settlementId, string pdfUrl)
    {
        var settlement = await _repo.GetByIdAsync(settlementId)
            ?? throw new NotFoundException("Settlement not found.");

        settlement.PdfUrl = pdfUrl;
        settlement.UpdatedAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync();
    }

    private static CarrierSettlementDto Map(CarrierSettlement settlement)
    {
        return new CarrierSettlementDto
        {
            Id = settlement.Id,
            LoadId = settlement.LoadId,
            CarrierId = settlement.CarrierId,
            SettlementNumber = settlement.SettlementNumber,
            SettlementDate = settlement.SettlementDate,
            Status = settlement.Status,
            TotalAmount = settlement.TotalAmount,
            Notes = settlement.Notes,
            DueDate = settlement.DueDate,

            PdfUrl = settlement.PdfUrl,

            LineItems = settlement.LineItems
                .Select(x => new CarrierSettlementLineItemDto
                {
                    Description = x.Description,
                    Qty = x.Qty,
                    Price = x.Price,
                    Amount = x.Amount,
                    Notes = x.Notes
                })
                .ToList()
        };
    }
    private void RecalculateDraftSettlement(CarrierSettlement settlement, Load load)
    {
        if (settlement.Status != SettlementStatus.Draft)
            return;

        var items = BuildLineItemsFromLoad(load);

        settlement.LineItems = items;
        settlement.TotalAmount = items.Sum(x => x.Amount);

        // Rillogarit Due Date sipas Carrier Terms
        settlement.DueDate = settlement.SettlementDate.AddDays(load.Carrier.PaymentTermsDays);
    }



}
