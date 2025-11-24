using LogisticsPlatform.Application.DTOs.Customers;
using LogisticsPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customers;

        public CustomersController(ICustomerService customers)
        {
            _customers = customers;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _customers.GetAllAsync();
            return Ok(list);
        }

        // GET: api/customers/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _customers.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerDto dto)
        {
            var customer = await _customers.CreateAsync(dto);
            return Ok(customer);
        }

        // PUT: api/customers/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateCustomerDto dto)
        {
            var updated = await _customers.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _customers.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return Ok("Customer deleted");
        }
    }
}
