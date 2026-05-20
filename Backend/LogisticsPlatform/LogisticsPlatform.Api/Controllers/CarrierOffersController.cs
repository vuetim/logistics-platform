using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/carrier-offers")]
public class CarrierOffersController : ControllerBase
{
    private readonly ICarrierAssignmentService _service;

    public CarrierOffersController(ICarrierAssignmentService service)
    {
        _service = service;
    }

    [HttpGet("open")]
    public async Task<IActionResult> GetOpen()
    {
        var items = await _service.GetOpenOffersAsync(User.GetUserId());
        return Ok(items);
    }
}
