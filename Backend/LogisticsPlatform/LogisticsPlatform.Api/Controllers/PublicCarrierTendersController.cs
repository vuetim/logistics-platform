using LogisticsPlatform.Application.DTOs.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/public/carrier-tenders")]
public class PublicCarrierTendersController : ControllerBase
{
    private readonly ICarrierAssignmentService _service;

    public PublicCarrierTendersController(ICarrierAssignmentService service)
    {
        _service = service;
    }

    [HttpGet("{token}")]
    public async Task<IActionResult> Get(string token)
    {
        var tender = await _service.GetPublicTenderAsync(token);
        return Ok(tender);
    }

    [HttpPost("{token}/accept")]
    public async Task<IActionResult> Accept(string token, RespondCarrierTenderDto dto)
    {
        await _service.AcceptPublicTenderAsync(token, dto);
        return NoContent();
    }

    [HttpPost("{token}/reject")]
    public async Task<IActionResult> Reject(string token, RespondCarrierTenderDto dto)
    {
        await _service.RejectPublicTenderAsync(token, dto);
        return NoContent();
    }
}
