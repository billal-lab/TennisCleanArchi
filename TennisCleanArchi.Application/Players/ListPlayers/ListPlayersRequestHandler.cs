using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TennisCleanArchi.Application.Common;
using TennisCleanArchi.Application.Data;

namespace TennisCleanArchi.Application.Players.ListPlayers;

public class ListPlayersRequestHandler : IRequestHandler<ListPlayersRequest, PagedResult<PlayerDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public ListPlayersRequestHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<PagedResult<PlayerDto>> Handle(ListPlayersRequest request, CancellationToken cancellationToken)
    {
        var items = await _dbContext.Players
            .AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderBy(p => p.Data.Rank)
            .Select(p => p.Adapt<PlayerDto>())
            .ToListAsync(cancellationToken);

        var totalItems = await _dbContext.Players.CountAsync(cancellationToken);

        return new PagedResult<PlayerDto>(request.PageNumber, request.PageSize, totalItems, items);
    }
}
