using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Notifications;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Entities.Financial;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
using SendGrid.Helpers.Errors.Model;
using ForbiddenException = LogisticsPlatform.Application.Common.Exceptions.ForbiddenException;

namespace LogisticsPlatform.Application.Services.Financial;

public class CarrierSettlementService : ICarrierSettlementService
{
    private readonly ICarrierSettlementRepository _repo;
    private readonly ILoadRepository _loads;
    private readonly IPermissionService _permission;
    private readonly INotificationService _notifications;

    public CarrierSettlementService(
        ICarrierSettlementRepository repo,
        ILoadRepository loads,
        IPermissionService permission,
        INotificationService notifications)
    {
        _repo = repo;
        _loads = loads;
        _permission = permission;
        _notifications = notifications;
    }

    public async Task<CarrierSettlementDto> GetAsync(Guid loadId)
    {
        var existing = await _repo.GetByLoadIdAsync(loadId);
        if (existing == null)
            throw new NotFoundException("Settlement not found.");

        return Map(existing);
    }

    public async Task<List<CarrierSettlementDto>> ListAsync()
    {
        var settlements = await _repo.ListAsync();
        return settlements.Select(Map).ToList();
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
        if (!await _permission.HasPermissionAsync(userId, Permission.Financial_Settlement_UpdateStatus))
            throw new ForbiddenException("You are not allowed to update settlement status.");

        var settlement = await _repo.GetByIdAsync(settlementId)
            ?? throw new NotFoundException("Settlement not found.");

        settlement.Status = status;
        settlement.UpdatedAt = DateTime.UtcNow;
        settlement.UpdatedByUserId = userId;

        await _repo.SaveChangesAsync();
        await _notifications.NotifySettlementEventAsync(
            settlement.LoadId,
            userId,
            $"Settlement {settlement.SettlementNumber} status changed to {status}");
    }

    public async Task<CarrierSettlementDto> RecordPaymentAsync(Guid settlementId, RecordSettlementPaymentDto dto, Guid userId)
    {
        if (!await _permission.HasPermissionAsync(userId, Permission.Financial_Settlement_RecordPayment))
            throw new ForbiddenException("You are not allowed to record settlement payments.");

        var settlement = await _repo.GetByIdAsync(settlementId)
            ?? throw new NotFoundException("Settlement not found.");

        var amountPaid = dto.AmountPaid < 0 ? 0 : dto.AmountPaid;
        settlement.AmountPaid = amountPaid > settlement.TotalAmount ? settlement.TotalAmount : amountPaid;
        settlement.PaidAt = settlement.AmountPaid >= settlement.TotalAmount
            ? dto.PaidAt ?? DateTime.UtcNow
            : dto.PaidAt;
        settlement.PaymentReference = dto.PaymentReference;
        settlement.Status = settlement.AmountPaid >= settlement.TotalAmount
            ? SettlementStatus.Paid
            : SettlementStatus.Sent;
        settlement.UpdatedAt = DateTime.UtcNow;
        settlement.UpdatedByUserId = userId;

        await _repo.SaveChangesAsync();
        await _notifications.NotifySettlementEventAsync(
            settlement.LoadId,
            userId,
            $"Payment recorded for settlement {settlement.SettlementNumber}: {settlement.AmountPaid:N2}");
        return Map(settlement);
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
            AmountPaid = settlement.AmountPaid,
            BalanceDue = settlement.TotalAmount - settlement.AmountPaid,
            PaidAt = settlement.PaidAt,
            PaymentReference = settlement.PaymentReference,
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
    private async Task RecalculateDraftSettlementAsync(CarrierSettlement settlement, Load load)
    {
        if (settlement.Status != SettlementStatus.Draft)
            return;

        var items = BuildLineItemsFromLoad(load);

        await _repo.DeleteLineItemsBySettlementIdAsync(settlement.Id);
        foreach (var item in items)
        {
            item.SettlementId = settlement.Id;
        }
        if (items.Count > 0)
        {
            await _repo.AddLineItemsAsync(items);
        }

        settlement.TotalAmount = items.Sum(x => x.Amount);
        if (settlement.AmountPaid > settlement.TotalAmount)
            settlement.AmountPaid = settlement.TotalAmount;

        // Rillogarit Due Date sipas Carrier Terms
        settlement.DueDate = settlement.SettlementDate.AddDays(load.Carrier.PaymentTermsDays);
    }



}
