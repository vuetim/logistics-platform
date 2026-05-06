using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers;

[ApiController]
[Route("api/orders/{orderId:guid}/routes")]
[Authorize]
public class OrderRoutesController : ControllerBase
{
    private readonly IOrderRouteService _service;

    public OrderRoutesController(IOrderRouteService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        Guid orderId,
        [FromBody] CreateOrderRouteDto dto)
    {
        await _service.CreateAsync(orderId, dto);
        return CreatedAtAction(nameof(GetAll), new { orderId }, null);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid orderId)
    {
        var routes = await _service.GetByOrderIdAsync(orderId);
        return Ok(routes);
    }

    [HttpPut("{routeId:guid}")]
    public async Task<IActionResult> Update(
        Guid routeId,
        [FromBody] UpdateOrderRouteDto dto)
    {
        await _service.UpdateAsync(routeId, dto);
        return NoContent();
    }

    [HttpDelete("{routeId:guid}")]
    public async Task<IActionResult> Delete(Guid routeId)
    {
        await _service.DeleteAsync(routeId);
        return NoContent();
    }
}
