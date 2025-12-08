using LogisticsPlatform.Application.DTOs.Orders.Equipment;
using LogisticsPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/orders/{orderId:guid}/equipment")]
    public class OrderEquipmentRequirementsController : ControllerBase
    {
        private readonly IOrderEquipmentRequirementService _service;

        public OrderEquipmentRequirementsController(IOrderEquipmentRequirementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid orderId)
        {
            var result = await _service.GetByOrderAsync(orderId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            Guid orderId,
            [FromBody] CreateOrderEquipmentRequirementDto dto)
        {
            var created = await _service.CreateAsync(orderId, dto);
            return CreatedAtAction(nameof(Get), new { orderId }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateOrderEquipmentRequirementDto dto)
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
}
