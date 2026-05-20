using LogisticsPlatform.Application.DTOs.Loads.Exceptions;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/loads/{loadId:guid}/exceptions")]
public class LoadExceptionsController : ControllerBase
{
    private readonly ILoadExceptionService _service;

    public LoadExceptionsController(ILoadExceptionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetByLoad(Guid loadId)
    {
        var items = await _service.GetByLoadAsync(loadId, User.GetUserId());
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid loadId, CreateLoadExceptionRequest request)
    {
        var id = await _service.CreateAsync(loadId, request, User.GetUserId());
        return Ok(new { Id = id });
    }

    [HttpPut("{exceptionId:guid}")]
    public async Task<IActionResult> Update(Guid loadId, Guid exceptionId, UpdateLoadExceptionRequest request)
    {
        await _service.UpdateAsync(loadId, exceptionId, request, User.GetUserId());
        return NoContent();
    }
}
