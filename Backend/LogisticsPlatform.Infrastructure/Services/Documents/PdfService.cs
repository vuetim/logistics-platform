using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities.Financial;
using LogisticsPlatform.Infrastructure.Services.Documents;
using QuestPDF.Fluent;
using Microsoft.AspNetCore.Hosting;


namespace LogisticsPlatform.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        private readonly IWebHostEnvironment _env;

        public PdfService(IWebHostEnvironment env)
        {
            _env = env;
        }


        public byte[] GenerateCustomerInvoicePdf(CustomerInvoice invoice)
        {
            var document = new InvoicePdfDocument(invoice);

            return document.GeneratePdf();
        }
        public byte[] GenerateCarrierSettlementPdf(CarrierSettlement settlement)
        {
            var document = new CarrierSettlementPdfDocument(settlement);
            return document.GeneratePdf();
        }
        public async Task<string> SaveCarrierSettlementPdfAsync(CarrierSettlement settlement)
        {
            var pdfBytes = GenerateCarrierSettlementPdf(settlement);

            var folder = Path.Combine(_env.WebRootPath, "uploads", "carrier-settlements", settlement.Id.ToString());
            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, "settlement.pdf");
            await File.WriteAllBytesAsync(filePath, pdfBytes);

            return $"/uploads/carrier-settlements/{settlement.Id}/settlement.pdf";
        }


    }
}
