using MediatR;
using TennisCleanArchi.Application.Common;

namespace TennisCleanArchi.Application.Players.ListPlayers;

public record ListPlayersRequest : IRequest<PagedResult<PlayerDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
