using LogisticsPlatform.Application.DTOs.Carriers.Contacts;
using LogisticsPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/carrier-contacts")]
    [Authorize] // Require JWT
    public class CarrierContactsController : ControllerBase
    {
        private readonly ICarrierContactService _service;

        public CarrierContactsController(ICarrierContactService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarrierContactDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCarrierContactDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _service.DeleteAsync(id)
                ? Ok("Deleted successfully")
                : NotFound();
        }

        [HttpGet("carrier/{carrierId}")]
        public async Task<IActionResult> GetByCarrier(Guid carrierId)
        {
            return Ok(await _service.GetByCarrierAsync(carrierId));
        }
    }
}
