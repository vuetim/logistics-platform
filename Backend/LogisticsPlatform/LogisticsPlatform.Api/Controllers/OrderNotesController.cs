using LogisticsPlatform.Application.DTOs.Orders.Notes;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/orders/{orderId:guid}/notes")]
    public class OrderNotesController : ControllerBase
    {
        private readonly IOrderNoteService _service;

        public OrderNotesController(IOrderNoteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            var userId = User.GetUserId();
            var notes = await _service.GetByOrderAsync(orderId, userId);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid orderId, [FromBody] CreateOrderNoteDto dto)
        {
            var userId = User.GetUserId();
            var note = await _service.CreateAsync(orderId, dto, userId);
            return Ok(note);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderNoteDto dto)
        {
            var userId = User.GetUserId();
            var note = await _service.UpdateAsync(id, dto, userId);
            if (note == null) return NotFound();
            return Ok(note);
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
