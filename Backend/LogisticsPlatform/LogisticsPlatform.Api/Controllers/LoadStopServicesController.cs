using LogisticsPlatform.Application.DTOs.Loads.LoadStopServices;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/load-stops/{stopId:guid}/services")]
public class LoadStopServicesController : ControllerBase
{
    private readonly ILoadStopServiceRequirementService _service;

    public LoadStopServicesController(ILoadStopServiceRequirementService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetByStop(Guid stopId)
    {
        var items = await _service.GetByStopAsync(stopId, User.GetUserId());
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid stopId, CreateLoadStopServiceRequest request)
    {
        var id = await _service.CreateAsync(stopId, request, User.GetUserId());
        return Ok(new { Id = id });
    }

    [HttpDelete("{serviceId:guid}")]
    public async Task<IActionResult> Delete(Guid stopId, Guid serviceId)
    {
        await _service.DeleteAsync(stopId, serviceId, User.GetUserId());
        return NoContent();
    }
}
