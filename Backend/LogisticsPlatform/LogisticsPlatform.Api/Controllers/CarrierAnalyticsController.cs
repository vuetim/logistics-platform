using LogisticsPlatform.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/carriers")]
[Authorize]
public class CarrierAnalyticsController : ControllerBase
{
    private readonly CarrierScoreCardService _scorecardService;

    public CarrierAnalyticsController(CarrierScoreCardService scorecardService)
    {
        _scorecardService = scorecardService;
    }

    [HttpGet("{carrierId}/scorecard")]
    public async Task<IActionResult> GetScorecard(Guid carrierId)
    {
        var scorecard = await _scorecardService.GetScorecardAsync(carrierId);
        return Ok(scorecard);
    }
}
