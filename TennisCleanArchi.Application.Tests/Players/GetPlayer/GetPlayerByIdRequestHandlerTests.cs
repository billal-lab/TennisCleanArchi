using TennisCleanArchi.Application.Common.Data;
using TennisCleanArchi.Application.Common.Exceptions;
using TennisCleanArchi.Application.Players.GetPlayerById;
using TennisCleanArchi.Domain;
using TennisCleanArchi.Shared;

namespace TennisCleanArchi.Application.Tests.Players.GetPlayer;

public class GetPlayerByIdRequestHandlerTests : BaseTests
{
    private readonly IApplicationDbContext _dbContext;

    public GetPlayerByIdRequestHandlerTests()
    {
        _dbContext =  Fixture.DbContext;

        SeedData();
    }

    private void SeedData()
    {
        var player = new Player
        {
            Id = 1,
            FirstName = "Novak",
            LastName = "Djokovic",
            ShortName = "N.DJO",
            Picture = "https://tenisu.latelier.co/resources/Djokovic.png",
            Country = new Country { Code = "SRB", Picture = "https://tenisu.latelier.co/resources/Serbie.png" },
            Sex = Sex.Male,
            Data = new PlayerStats
            {
                Rank = 2,
                Points = 2542,
                Weight = 80000,
                Height = 188,
                Age = 31,
                Last = [1, 1, 1, 1, 1]
            }
        };

        _dbContext.Players.Add(player);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Handle_WithExistingPlayerId_ReturnsPlayer()
    {
        var handler = new GetPlayerByIdRequestHandler(_dbContext);
        var request = new GetPlayerByIdRequest { Id = 1 };
        
        var result = await handler.Handle(request, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Novak", result.FirstName);
        Assert.Equal("Djokovic", result.LastName);
    }

    [Fact]
    public async Task Handle_WithNonExistingPlayerId_ThrowsNotFoundException()
    {
        // Arrange
        var handler = new GetPlayerByIdRequestHandler(_dbContext);
        var request = new GetPlayerByIdRequest { Id = 999 };
        
        await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(request, CancellationToken.None));
    }
}
