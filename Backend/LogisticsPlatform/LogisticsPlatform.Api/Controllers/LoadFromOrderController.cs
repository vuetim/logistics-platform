using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Authorize]
[Route("api/loads/from-order")]
public class LoadFromOrderController : ControllerBase
{
    private readonly ILoadService _loadService;

    public LoadFromOrderController(ILoadService loadService)
    {
        _loadService = loadService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLoadFromOrderDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var loadId = await _loadService.CreateFromOrderAsync(dto, userId);
        return Ok(new { loadId });
    }
}
