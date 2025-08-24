using MediatR;

namespace TennisCleanArchi.Application.Players.GetPlayerById;

public class GetPlayerByIdRequest : IRequest<PlayerDetailedDto>
{
    public int Id { get; init; }
}
