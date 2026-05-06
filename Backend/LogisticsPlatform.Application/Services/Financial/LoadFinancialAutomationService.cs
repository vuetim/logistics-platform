using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using SendGrid.Helpers.Errors.Model;

namespace LogisticsPlatform.Application.Services.Financial;

public class LoadFinancialAutomationService : ILoadFinancialAutomationService
{
    private readonly ICustomerInvoiceService _customerInvoiceService;
    private readonly ICarrierSettlementService _carrierSettlementService;

    public LoadFinancialAutomationService(
        ICustomerInvoiceService customerInvoiceService,
        ICarrierSettlementService carrierSettlementService)
    {
        _customerInvoiceService = customerInvoiceService;
        _carrierSettlementService = carrierSettlementService;
    }

    public async Task GenerateFinancialDocumentsAsync(Load load)
    {
        // GetAsync is intentionally idempotent: it returns an existing document
        // or creates a draft one if it does not exist.
        await _customerInvoiceService.GetAsync(load.Id);

        if (load.CarrierId != null)
        {
            try
            {
                await _carrierSettlementService.GetAsync(load.Id);
            }
            catch (BusinessValidationException)
            {
                // Completing the customer-facing load should not fail only because
                // carrier settlement data is not ready yet. Billing tab surfaces the reason.
            }
        }
    }
}
