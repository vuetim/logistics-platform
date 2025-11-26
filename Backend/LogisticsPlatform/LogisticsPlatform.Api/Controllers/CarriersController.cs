using LogisticsPlatform.Application.DTOs.Carriers;
using LogisticsPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // mundesh me hekë për testim nëse don
    public class CarriersController : ControllerBase
    {
        private readonly ICarrierService _service;

        public CarriersController(ICarrierService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carriers = await _service.GetAllAsync();
            return Ok(carriers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var carrier = await _service.GetByIdAsync(id);
            if (carrier == null) return NotFound();
            return Ok(carrier);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarrierDto dto)
        {
            var carrier = await _service.CreateAsync(dto);
            return Ok(carrier);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCarrierDto dto)
        {
            var carrier = await _service.UpdateAsync(id, dto);
            if (carrier == null) return NotFound();
            return Ok(carrier);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok("Deleted");
        }
    }
}
