using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers.Financial;

[ApiController]
[Route("api/financials/settlements/{settlementId:guid}/status")]
[Authorize]
public class CarrierSettlementStatusController : ControllerBase
{
    private readonly ICarrierSettlementService _service;

    public CarrierSettlementStatusController(ICarrierSettlementService service)
    {
        _service = service;
    }

    [HttpGet("/api/financials/settlements")]
    public async Task<IActionResult> List()
    {
        var settlements = await _service.ListAsync();
        return Ok(settlements);
    }

    // PATCH: /api/financials/settlements/{settlementId}/status
    [HttpPatch]
    public async Task<IActionResult> UpdateStatus(Guid settlementId, [FromBody] UpdateSettlementStatusDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.UpdateStatusAsync(settlementId, dto.Status, userId);
        return NoContent();
    }

    [HttpPatch("/api/financials/settlements/{settlementId:guid}/payment")]
    public async Task<IActionResult> RecordPayment(Guid settlementId, [FromBody] RecordSettlementPaymentDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var settlement = await _service.RecordPaymentAsync(settlementId, dto, userId);
        return Ok(settlement);
    }
}
