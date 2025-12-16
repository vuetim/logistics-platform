using LogisticsPlatform.Application.DTOs.Loads.LoadEquipment;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers;

[ApiController]
[Route("api/loads/{loadId:guid}/equipment")]
[Authorize]
public class LoadEquipmentController : ControllerBase
{
    private readonly ILoadEquipmentService _service;

    public LoadEquipmentController(ILoadEquipmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid loadId)
    {
        return Ok(await _service.GetByLoadAsync(loadId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid loadId, CreateLoadEquipmentDto dto)
    {
        var result = await _service.CreateAsync(loadId, dto);
        return CreatedAtAction(nameof(Get), new { loadId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateLoadEquipmentDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
