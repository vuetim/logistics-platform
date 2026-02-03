using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/customer-addresses")]
public class CustomerAddressesController : ControllerBase
{
    private readonly ICustomerAddressService _service;

    public CustomerAddressesController(ICustomerAddressService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerAddressDto dto)
    {
        await _service.CreateAsync(dto);
        return NoContent();
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomer(Guid customerId)
    {
        var result = await _service.GetByCustomerAsync(customerId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCustomerAddressDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
