using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Services;
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

    [HttpPatch("{invoiceId:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid invoiceId, UpdateInvoiceStatusDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.UpdateStatusAsync(invoiceId, dto.Status, userId);

        return NoContent();
    }
}
