using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TennisCleanArchi.Host.Controllers;

public class StatsController : BaseController
{
    public StatsController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet("Summary")]
    [SwaggerOperation(Summary = "Get summary statistics", Description = "Country with best win ratio.\n\nAverage BMI.\n\nMedian height of players.")]

    public async Task<IActionResult> GetSummaryStats(CancellationToken cancellationToken)
    {
        var query = new Application.Stats.GetStats.GetSummaryStatsRequest
        {
        };

        var stats = await _mediator.Send(query, cancellationToken);
        return Ok(stats);
    }
}
