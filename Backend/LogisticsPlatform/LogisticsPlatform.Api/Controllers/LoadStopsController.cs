using LogisticsPlatform.Application.DTOs.Loads.LoadStop;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers;

[ApiController]
[Route("api/loads/{loadId}/stops")]
[Authorize]
public class LoadStopsController : ControllerBase
{
    private readonly ILoadStopService _service;

    public LoadStopsController(ILoadStopService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid loadId, CreateLoadStopDto dto)
    {
        await _service.AddAsync(loadId, dto);
        return Ok();
    }

    [HttpPut("{stopId}")]
    public async Task<IActionResult> Update(Guid stopId, UpdateLoadStopDto dto)
    {
        await _service.UpdateAsync(stopId, dto);
        return Ok();
    }

    [HttpDelete("{stopId}")]
    public async Task<IActionResult> Delete(Guid stopId)
    {
        await _service.DeleteAsync(stopId);
        return NoContent();
    }
}
