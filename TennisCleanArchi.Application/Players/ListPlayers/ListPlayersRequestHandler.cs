using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TennisCleanArchi.Application.Data;

namespace TennisCleanArchi.Application.Players.ListPlayers;

public class ListPlayersRequestHandler : IRequestHandler<ListPlayersRequest, List<PlayerDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public ListPlayersRequestHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<List<PlayerDto>> Handle(ListPlayersRequest request, CancellationToken cancellationToken)
    {
        return await _dbContext.Players
            .AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderBy(p => p.Data.Rank)
            .Select(p => p.Adapt<PlayerDto>())
            .ToListAsync(cancellationToken);
    }
}
