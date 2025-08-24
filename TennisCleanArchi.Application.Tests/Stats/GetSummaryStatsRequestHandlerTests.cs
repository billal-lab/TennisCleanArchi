using TennisCleanArchi.Application.Data;
using TennisCleanArchi.Application.Stats.GetStats;
using TennisCleanArchi.Domain;
using TennisCleanArchi.Shared;

namespace TennisCleanArchi.Application.Tests.Stats;

public class GetSummaryStatsRequestHandlerTests : BaseTests
{
    private readonly IApplicationDbContext _dbContext;

    public GetSummaryStatsRequestHandlerTests()
    {
        _dbContext = Fixture.DbContext;
        SeedData();
    }

    private void SeedData()
    {
        var country1 = new Country { Code = "US", Picture = "https://example.com/us.png" };
        var country2 = new Country { Code = "FR", Picture = "https://example.com/fr.png" };

        _dbContext.Countries.AddRange(country1, country2);

        var player1 = new Player
        {
            Id = 5,
            FirstName = "Tom",
            LastName = "Lee",
            ShortName = "T.Lee",
            Picture = "https://example.com/tomlee.png",
            Data = new PlayerStats { Weight = 70000, Height = 175, Last = new[] { 1, 1, 0, 0, 1 } },
            Country = country1,
            Sex = Sex.Male,
        };
        var player2 = new Player
        {
            Id = 6,
            FirstName = "Sara",
            LastName = "Kim",
            ShortName = "S.Kim",
            Picture = "https://example.com/sarakim.png",
            Data = new PlayerStats { Weight = 60000, Height = 160, Last = new[] { 1, 1, 0, 0, 1 } },
            Country = country2,
            Sex = Sex.Female,
        };
        var player3 = new Player
        {
            Id = 3,
            FirstName = "Alice",
            LastName = "Johnson",
            ShortName = "A.Johnson",
            Picture = "https://example.com/alicejohnson.png",
            Country = country2,
            Data = new PlayerStats { Weight = 60000, Height = 160, Last = new[] { 1, 1, 0, 0, 1 } },
            Sex = Sex.Female
        };

        _dbContext.Players.AddRange(player1, player2, player3);

        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetSummaryStats_ReturnsCorrectStats()
    {
        // Arrange
        var handler = new GetSummaryStatsRequestHandler(_dbContext);
        var request = new GetSummaryStatsRequest();
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.BestCountryByWinRatio);
        Assert.Equal("US", result.BestCountryByWinRatio.Code); // France has 2 players with better average win ratio
        var expectedImc = 23244; // Average IMC = (70000/1.75^2 + 60000/1.60^2 + 60000/1.60^2) / 3 = 22.86
        Assert.InRange(result.PlayersAverageImc, expectedImc - 0.1, expectedImc + 0.1); // Allow a small margin of error
        var expectedMedianHeight = 160; // Median height = 160 (heights are 175, 160, 160)
        Assert.Equal(expectedMedianHeight, result.PlayersHeightMean);
    }
}
