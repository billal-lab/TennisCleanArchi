using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TennisCleanArchi.Application.Common.Data;
using TennisCleanArchi.Application.Common.Exceptions;

namespace TennisCleanArchi.Application.Players.AddPlayer;

public class AddPlayerRequestHandler : IRequestHandler<AddPlayerRequest, int>
{
    private readonly IApplicationDbContext _dbContext;

    public AddPlayerRequestHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(AddPlayerRequest request, CancellationToken cancellationToken)
    {
        // Create the player by mapping the request to the Player domain model
        var player = request.Adapt<Domain.Player>();

        if(!await _dbContext.Countries.AnyAsync(c => c.Code == request.CountryCode, cancellationToken))
        {
            throw new NotFoundException($"Country with code {request.CountryCode} not found");
        }

        // Check if a player with the same rank
        // TODO: Check p.Sex.Value == request.Sex ?
        if (await _dbContext.Players.AnyAsync(p => p.Data.Rank == request.Data.Rank, cancellationToken))
        {
            throw new ConflictException($"Player with rank {request.Data.Rank} already exists");
        }

        // Add the new player to the database context
        await _dbContext.Players.AddAsync(player, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return player.Id;
    }
}
