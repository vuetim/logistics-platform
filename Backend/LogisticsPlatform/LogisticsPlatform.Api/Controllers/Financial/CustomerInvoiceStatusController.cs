using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/financials/invoices")]
[Authorize]
public class CustomerInvoiceStatusController : ControllerBase
{
    private readonly ICustomerInvoiceService _service;

    public CustomerInvoiceStatusController(ICustomerInvoiceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var invoices = await _service.ListAsync();
        return Ok(invoices);
    }

    [HttpPatch("{invoiceId:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid invoiceId, UpdateInvoiceStatusDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.UpdateStatusAsync(invoiceId, dto.Status, userId);

        return NoContent();
    }

    [HttpPatch("{invoiceId:guid}/payment")]
    public async Task<IActionResult> RecordPayment(Guid invoiceId, [FromBody] RecordInvoicePaymentDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var invoice = await _service.RecordPaymentAsync(invoiceId, dto, userId);
        return Ok(invoice);
    }
}
