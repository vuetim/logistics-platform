using LogisticsPlatform.Application.DTOs.Customers.Contacts;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerContactsController : ControllerBase
    {
        private readonly ICustomerContactService _service;

        public CustomerContactsController(ICustomerContactService service)
        {
            _service = service;
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(Guid customerId)
        {
            var contacts = await _service.GetByCustomerAsync(customerId);
            return Ok(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerContactDto dto)
        {
            var contact = await _service.CreateAsync(dto);
            return Ok(contact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCustomerContactDto dto)
        {
            var contact = await _service.UpdateAsync(id, dto);
            if (contact == null) return NotFound();

            return Ok(contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ?NoContent() : NotFound();
        }

        [HttpGet("positions")]
        public IActionResult GetContactPositions()
        {
            return Ok(CustomerContactRoles.All);
        }


    }
}
