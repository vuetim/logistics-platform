using LogisticsPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers.Financial;

[ApiController]
[Route("api/loads/{loadId:guid}/financials")]
[Authorize]
public class LoadFinancialSnapshotController : ControllerBase
{
    private readonly ILoadFinancialSnapshotService _service;

    public LoadFinancialSnapshotController(ILoadFinancialSnapshotService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetSnapshot(Guid loadId) 
    {
        var result = await _service.GetSnapshotAsync(loadId);
        return Ok(result);
    }
}
