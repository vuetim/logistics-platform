using System.Security.Claims;
using LogisticsPlatform.Application.DTOs.Costs;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/orders/{orderId:guid}/costs")]
public class OrderCostsController : ControllerBase
{
    private readonly IOrderCostService _service;

    public OrderCostsController(IOrderCostService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid orderId)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var result = await _service.GetAsync(orderId, userId);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put(Guid orderId, [FromBody] UpdateOrderCostDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        await _service.UpdateAsync(orderId, dto, userId);
        return NoContent();
    }
}
