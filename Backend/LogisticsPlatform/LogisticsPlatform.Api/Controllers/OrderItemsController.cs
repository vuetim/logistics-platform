using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers;

[ApiController]
[Route("api/orders/{orderId:guid}/items")]
[Authorize]
public class OrderItemsController : ControllerBase
{
    private readonly IOrderItemService _service;

    public OrderItemsController(IOrderItemService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Guid orderId, CreateOrderItemDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var itemId = await _service.AddAsync(orderId, dto, userId);

        return CreatedAtAction(nameof(GetAll), new { orderId }, null);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid orderId)
    {
        var items = await _service.GetByOrderIdAsync(orderId);
        return Ok(items);
    }
}
