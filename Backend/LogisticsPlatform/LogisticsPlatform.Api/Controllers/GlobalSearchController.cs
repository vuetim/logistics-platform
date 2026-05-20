using LogisticsPlatform.Application.Interfaces.Services.Search;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/search")]
public class GlobalSearchController : ControllerBase
{
    private readonly IGlobalSearchService _search;

    public GlobalSearchController(IGlobalSearchService search)
    {
        _search = search;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int take = 8)
    {
        var results = await _search.SearchAsync(q ?? string.Empty, User.GetUserId(), take);
        return Ok(results);
    }
}
