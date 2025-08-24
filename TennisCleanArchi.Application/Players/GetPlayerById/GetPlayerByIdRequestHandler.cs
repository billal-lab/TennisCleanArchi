using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TennisCleanArchi.Application.Common.Data;
using TennisCleanArchi.Application.Common.Exceptions;

namespace TennisCleanArchi.Application.Players.GetPlayerById;

public class GetPlayerByIdRequestHandler : IRequestHandler<GetPlayerByIdRequest, PlayerDetailedDto>
{

    private readonly IApplicationDbContext _dbContext;
    public GetPlayerByIdRequestHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PlayerDetailedDto> Handle(GetPlayerByIdRequest request, CancellationToken cancellationToken)
    {
        return await _dbContext.Players
            .AsNoTracking()
            .Include(p => p.Country)
            .Where(p => p.Id == request.Id)
            .Select(p => p.Adapt<PlayerDetailedDto>())
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Player with Id {request.Id} not found.");
    }
}
