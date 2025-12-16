using LogisticsPlatform.Application.DTOs;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/delay-responsibility")]
[Authorize] // 🔐 JWT only
public class DelayResponsibilityController : ControllerBase
{
    private readonly IDelayResponsibilityService _service;

    public DelayResponsibilityController(IDelayResponsibilityService service)
    {
        _service = service;
    }

    // ============================
    // MANUAL ASSIGNMENT
    // ============================
    [HttpPost("{stopId}")]
    public async Task<IActionResult> Assign(
        Guid stopId,
        AssignDelayResponsibilityDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.AssignAsync(
            stopId,
            dto.Responsibility,
            dto.Reason,
            userId);

        return Ok();
    }

    // ============================
    // READ (AUDIT / UI)
    // ============================
    [HttpGet("load/{loadId}")]
    public async Task<IActionResult> GetByLoad(Guid loadId)
    {
        var result = await _service.GetByLoadAsync(loadId);
        return Ok(result);
    }
}
