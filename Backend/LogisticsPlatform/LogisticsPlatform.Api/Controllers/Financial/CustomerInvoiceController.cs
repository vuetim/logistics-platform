using LogisticsPlatform.Application.DTOs;
using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers.Financial
{
    [ApiController]
    [Route("api/loads/{loadId:guid}/financials/invoices")]
    [Authorize]
    public class CustomerInvoiceController : ControllerBase
    {
        private readonly ICustomerInvoiceService _service;
        private readonly IPdfService _pdf;
        private readonly IEmailService _emailService;

        public CustomerInvoiceController(
            ICustomerInvoiceService service,
            IPdfService pdf,
            IEmailService emailService)
        {
            _service = service;
            _pdf = pdf;
            _emailService = emailService;
        }

        // =======================
        // GET invoice by loadId
        // Turvo-style: Get OR Create
        // =======================
        [HttpGet]
        public async Task<IActionResult> Get(Guid loadId)
        {
            // Nëse ekziston invoice → kthehet
            // Nëse nuk ekziston → krijohet auto draft nga load (GetAsync = GetOrCreate)
            var result = await _service.GetAsync(loadId);
            return Ok(result);
        }

        // =======================
        // CREATE invoice (manual)
        // (nëse don me e bo explicit, përveç auto-create në GET)
        // =======================
        [HttpPost]
        public async Task<IActionResult> Create(Guid loadId, [FromBody] CreateInvoiceDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var created = await _service.CreateAsync(loadId, dto, userId);

            return Ok(created);
        }

        // =======================
        // SEND invoice PDF via email (manual, në çdo status)
        // =======================
        [HttpPost("{invoiceId:guid}/send")]
        public async Task<IActionResult> SendPdf(Guid loadId, Guid invoiceId, [FromBody] SendInvoiceEmailDto dto)
        {
            var invoiceEntity = await _service.GetByIdAsync(invoiceId);

            var pdfBytes = _pdf.GenerateCustomerInvoicePdf(invoiceEntity);

            await _emailService.SendAsync(
                to: dto.Email,
                subject: $"Invoice {invoiceEntity.InvoiceNumber}",
                body: "Please find attached your invoice.",
                attachmentName: $"invoice-{invoiceEntity.InvoiceNumber}.pdf",
                attachmentBytes: pdfBytes
            );

            return Ok(new { message = "Invoice sent successfully." });
        }

        // =======================
        // DOWNLOAD invoice PDF (manual generate)
        // Lejohet edhe pa delivered – gjeneron nga gjendja aktuale
        // =======================
        [HttpGet("{invoiceId:guid}/pdf")]
        public async Task<IActionResult> GetPdf(Guid loadId, Guid invoiceId)
        {
            var invoiceEntity = await _service.GetByIdAsync(invoiceId);

            var pdfBytes = _pdf.GenerateCustomerInvoicePdf(invoiceEntity);

            return File(pdfBytes, "application/pdf", $"invoice-{invoiceEntity.InvoiceNumber}.pdf");
        }
    }
}
