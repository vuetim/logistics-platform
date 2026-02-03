using LogisticsPlatform.Application.DTOs.Customers;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customers;
    private readonly ICustomerQueryService _queries;

    public CustomersController(
        ICustomerService customers,
        ICustomerQueryService queries)
    {
        _customers = customers;
        _queries = queries;
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
        return customer == null ? NotFound() : Ok(customer);
    }

    // GET: api/customers/{id}/details
    [HttpGet("{id:guid}/details")]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var customer = await _customers.GetDetailsAsync(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    // GET: api/customers/paged
    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] CustomersQueryParameters p)
    {
        var result = await _queries.GetPagedAsync(p);
        return Ok(result);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        var id = await _customers.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetDetails),
            new { id },
            null
        );
    }

    // POST: api/customers/full
    [HttpPost("full")]
    public async Task<IActionResult> CreateFull(CreateCustomerFullDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var id = await _customers.CreateFullAsync(dto, userId);

        return CreatedAtAction(
            nameof(GetDetails),
            new { id },
            null
        );
    }

    // PUT: api/customers/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCustomerDto dto)
    {
        var updated = await _customers.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/customers/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _customers.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
