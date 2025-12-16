using LogisticsPlatform.Application.DTOs.Carriers.Addresses;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/carrier-addresses")]
    [Authorize]
    public class CarrierAddressesController : ControllerBase
    {
        private readonly ICarrierAddressService _service;

        public CarrierAddressesController(ICarrierAddressService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarrierAddressDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCarrierAddressDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _service.DeleteAsync(id)
                ? Ok("Deleted")
                : NotFound();
        }

        [HttpGet("carrier/{carrierId}")]
        public async Task<IActionResult> GetByCarrier(Guid carrierId)
        {
            return Ok(await _service.GetByCarrierAsync(carrierId));
        }
    }
}
