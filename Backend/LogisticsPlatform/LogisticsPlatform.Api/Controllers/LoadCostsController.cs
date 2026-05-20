using System.Security.Claims;
using LogisticsPlatform.Application.DTOs.Costs;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/loads/{loadId:guid}/costs")]
public class LoadCostsController : ControllerBase
{
    private readonly ILoadCostService _service;

    public LoadCostsController(ILoadCostService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid loadId)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var result = await _service.GetAsync(loadId, userId);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put(Guid loadId, [FromBody] UpdateLoadCostDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        await _service.UpdateAsync(loadId, dto, userId);
        return NoContent();
    }
}
