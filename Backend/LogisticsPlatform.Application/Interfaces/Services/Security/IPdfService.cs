using LogisticsPlatform.Domain.Entities.Financial;

namespace LogisticsPlatform.Application.Interfaces.Services.Security
{
    public interface IPdfService
    {
        byte[] GenerateCustomerInvoicePdf(CustomerInvoice invoice);
        byte[] GenerateCarrierSettlementPdf(CarrierSettlement settlement);
        Task<string> SaveCarrierSettlementPdfAsync(CarrierSettlement fullSettlement);
    }
}
