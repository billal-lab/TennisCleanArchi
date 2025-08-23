using MediatR;

namespace TennisCleanArchi.Application.Players.ListPlayers;

public record ListPlayersRequest : IRequest<List<PlayerDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
