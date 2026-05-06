using LogisticsPlatform.Application.DTOs.Orders.ExternalIds;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/orders/{orderId:guid}/external-ids")]
    public class OrderExternalIdsController : ControllerBase
    {
        private readonly IOrderExternalIdService _service;

        public OrderExternalIdsController(IOrderExternalIdService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            var userId = User.GetUserId();
            var ids = await _service.GetByOrderAsync(orderId, userId);
            return Ok(ids);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid orderId, [FromBody] CreateOrderExternalIdDto dto)
        {
            var userId = User.GetUserId();
            var created = await _service.CreateAsync(orderId, dto, userId);
            return Ok(created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderExternalIdDto dto)
        {
            var userId = User.GetUserId();
            var updated = await _service.UpdateAsync(id, dto, userId);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            var deleted = await _service.DeleteAsync(id, userId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
