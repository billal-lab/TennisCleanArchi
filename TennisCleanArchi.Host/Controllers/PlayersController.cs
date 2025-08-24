using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TennisCleanArchi.Application.Players.AddPlayer;

namespace TennisCleanArchi.Host.Controllers;

public class PlayersController : BaseController
{
    public PlayersController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get a paginated list of players", Description = "Page size max is 100")]
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

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a player by ID")]
    public async Task<IActionResult> GetPlayerById(int id)
    {
        var query = new Application.Players.GetPlayerById.GetPlayerByIdRequest
        {
            Id = id
        };
        var player = await _mediator.Send(query);
        if (player == null)
        {
            return NotFound();
        }
        return Ok(player);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new player", Description = "Player rank must be unique.\n\nSex must be 'M' or 'F'")]
    public async Task<IActionResult> AddPlayer([FromBody] AddPlayerRequest request, CancellationToken cancellationToken)
    {
        var playerId = await _mediator.Send(request, cancellationToken);
        return new CreatedResult(nameof(GetPlayerById), new { id = playerId });
    }
}
