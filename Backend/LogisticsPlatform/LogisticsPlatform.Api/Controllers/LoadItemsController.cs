using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Authorize]
[Route("api/loads/{loadId}/items")]
public class LoadItemsController : ControllerBase
{
    private readonly ILoadItemService _service;

    public LoadItemsController(ILoadItemService service)
    {
        _service = service;
    }

    [HttpPut("{itemId}")]
    public async Task<IActionResult> Update(
        Guid loadId,
        Guid itemId,
        [FromBody] UpdateLoadItemDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        await _service.UpdateAsync(loadId, itemId, dto, userId);

        return NoContent(); 
    }
}
