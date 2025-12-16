using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Security;

namespace LogisticsPlatform.Application.Services.Financial;

public class LoadFinancialAutomationService : ILoadFinancialAutomationService
{
    private readonly ICustomerInvoiceService _customerInvoiceService;
    private readonly ICarrierSettlementService _carrierSettlementService;
    private readonly ICarrierSettlementRepository _carrierSettlementRepo;
    private readonly IPdfService _pdf;

    public LoadFinancialAutomationService(
        ICustomerInvoiceService customerInvoiceService,
        ICarrierSettlementService carrierSettlementService, ICarrierSettlementRepository carrierSettlementRepo )
    {
        _customerInvoiceService = customerInvoiceService;
        _carrierSettlementService = carrierSettlementService;
        _pdf = _pdf;
        _carrierSettlementRepo = carrierSettlementRepo;
    }

    public async Task GenerateFinancialDocumentsAsync(Load load)
    {
        var today = DateTime.UtcNow;
        var due = today.AddDays(30);

        // Auto Customer Invoice
        await _customerInvoiceService.CreateAsync(
            load.Id,
            new CreateInvoiceDto
            {
                InvoiceDate = today,
                DueDate = due,
                Notes = "Auto generated on load completion."
            },
            userId: load.CreatedByUserId // 
        );

        // Auto Carrier Settlement 
        // Auto Carrier Settlement 
        if (load.CarrierId != null)
        {
            var settlement = await _carrierSettlementService.CreateAsync(
                load.Id,
                new CreateSettlementDto
                {
                    SettlementDate = today,
                    Notes = "Auto generated on load completion."
                },
                userId: load.CreatedByUserId
            );

            var fullSettlement = await _carrierSettlementService.GetByIdAsync(settlement.Id);

            var pdfUrl = await _pdf.SaveCarrierSettlementPdfAsync(fullSettlement);

            await _carrierSettlementService.UpdatePdfUrlAsync(fullSettlement.Id, pdfUrl);


        }
    }
}
