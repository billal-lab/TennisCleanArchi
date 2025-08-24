using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TennisCleanArchi.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatsController : ControllerBase
{
    private readonly IMediator _mediator;
    public StatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Summary")]

    public async Task<IActionResult> GetSummaryStats(CancellationToken cancellationToken)
    {
        var query = new Application.Stats.GetStats.GetSummaryStatsRequest
        {
        };

        var stats = await _mediator.Send(query, cancellationToken);
        return Ok(stats);
    }
}
