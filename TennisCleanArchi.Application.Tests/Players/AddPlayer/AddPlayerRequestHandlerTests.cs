using TennisCleanArchi.Application.Common.Exceptions;
using TennisCleanArchi.Application.Data;
using TennisCleanArchi.Application.Players.AddPlayer;

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
}
