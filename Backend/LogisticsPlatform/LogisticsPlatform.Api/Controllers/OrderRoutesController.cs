using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/orders/{orderId}/routes")]
[Authorize]
public class OrderRoutesController : ControllerBase
{
    private readonly IOrderRouteService _service;

    public OrderRoutesController(IOrderRouteService service)
    {
        _service = service;
    }

    // ➕ ADD ROUTE
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid orderId,
        CreateOrderRouteDto dto)
    {
        var id = await _service.CreateAsync(orderId, dto);
        return CreatedAtAction(nameof(GetAll), new { orderId }, null);
    }

    // 📄 GET ROUTES
    [HttpGet]
    public async Task<IActionResult> GetAll(Guid orderId)
    {
        var routes = await _service.GetByOrderIdAsync(orderId);
        return Ok(routes);
    }
}
