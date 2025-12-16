using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
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

    // UPDATE
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

    // COPY ORDER ITEM - LOAD ITEM
    [HttpPost("from-order/{orderId}/{orderItemId}")]
    public async Task<IActionResult> AddFromOrderItem(
        Guid loadId,
        Guid orderId,
        Guid orderItemId)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        await _service.AddFromOrderItemAsync(
            loadId, orderId, orderItemId, userId
        );

        return Ok(new { message = "Item copied from order." });
    }

    // DELETE
    [HttpDelete("{itemId}")]
    public async Task<IActionResult> Delete(Guid loadId, Guid itemId)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        await _service.DeleteAsync(loadId, itemId, userId);
        return NoContent();
    }
}
