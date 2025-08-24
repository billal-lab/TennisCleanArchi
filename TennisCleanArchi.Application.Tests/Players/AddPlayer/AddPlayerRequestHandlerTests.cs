using TennisCleanArchi.Application.Common.Data;
using TennisCleanArchi.Application.Common.Exceptions;
using TennisCleanArchi.Application.Players.AddPlayer;
using TennisCleanArchi.Shared;

namespace TennisCleanArchi.Application.Tests.Players.AddPlayer;

public class AddPlayerRequestHandlerTests : BaseTests
{
    private readonly IApplicationDbContext _dbContext;

    public AddPlayerRequestHandlerTests()
    {
        _dbContext = Fixture.DbContext;
        SeedData();
    }

    // Seed Data
    private void SeedData()
    {
        _dbContext.Countries.Add(new Domain.Country { Code = "USA", Picture = "http://example.com/usa.png" });
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsPlayerId()
    {
        // Arrange
        var handler = new AddPlayerRequestHandler(_dbContext);
        var request = new AddPlayerRequest
        {
            FirstName = "Roger",
            LastName = "Federer",
            CountryCode = "USA",
            Sex = "M",
            ShortName = "R.Fed",
            Picture = "http://example.com/picture.jpg",
            Data = new Domain.PlayerStats
            {
                Age = 40,
                Height = 185,
                Weight = 85,
                Points = 1000,
                Rank = 10,
                Last = [1, 1, 0, 1, 1]
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<int>(result);
    }

    [Fact]
    public async Task Handle_WithMissingCountry_ThrowsNotFoundException()
    {
        var handler = new AddPlayerRequestHandler(_dbContext);
        var request = new AddPlayerRequest
        {
            FirstName = "Roger",
            LastName = "Federer",
            CountryCode = "123", // Non-existent country
            Sex = "M",
            ShortName = "R.Fed",
            Picture = "http://example.com/picture.jpg",
            Data = new Domain.PlayerStats
            {
                Age = 40,
                Height = 185,
                Weight = 85,
                Points = 1000,
                Rank = 10,
                Last = [1, 1, 0, 1, 1]
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithDuplicateRank_ThrowsConflictException()
    {
        // Arrange
        var player1 = new Domain.Player
        {
            FirstName = "Mick",
            LastName = "Scott",
            ShortName = "M.Scott",
            Picture = "http://example.com/mick.png",
            CountryCode = "USA",
            Sex = Sex.Male,
            Data = new Domain.PlayerStats
            {
                Age = 30,
                Height = 180,
                Weight = 75,
                Points = 1500,
                Rank = 75, // Rank to be duplicated
                Last = [1, 0, 1, 1, 0]
            },
        };

        await _dbContext.Players.AddAsync(player1);
        await _dbContext.SaveChangesAsync();

        var request = new AddPlayerRequest
        {
            FirstName = "Max",
            LastName = "Smith",
            CountryCode = "USA",
            Sex = "M",
            ShortName =  "M.Smith",
            Picture = "http://example.com/picture.jpg",
            Data = new Domain.PlayerStats
            {
                Age = 40,
                Height = 185,
                Weight = 85,
                Points = 1000,
                Rank = 75,
                Last = [1, 1, 0, 1, 1]
            }
        };

        var handler = new AddPlayerRequestHandler(_dbContext);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(async () => await handler.Handle(request, CancellationToken.None));
    }
}
