using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TennisCleanArchi.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlayersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlayers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new Application.Players.ListPlayers.ListPlayersRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var players = await _mediator.Send(query);
        return Ok(players);
    }
}
