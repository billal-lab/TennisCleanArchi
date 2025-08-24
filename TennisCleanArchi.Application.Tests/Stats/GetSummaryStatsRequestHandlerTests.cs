using TennisCleanArchi.Application.Common.Caching;
using TennisCleanArchi.Application.Common.Data;
using TennisCleanArchi.Application.Stats.GetStats;
using TennisCleanArchi.Domain;
using TennisCleanArchi.Shared;

namespace TennisCleanArchi.Application.Tests.Stats;

public class GetSummaryStatsRequestHandlerTests : BaseTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICachingService _cachingService;

    public GetSummaryStatsRequestHandlerTests()
    {
        _dbContext = Fixture.DbContext;
        _cachingService = Fixture.CachingService;

        SeedData();
    }

    private void SeedData()
    {
        var us = new Country { Code = "US", Picture = "https://example.com/us.png" };
        var fr = new Country { Code = "FR", Picture = "https://example.com/fr.png" };

        _dbContext.Countries.AddRange(us, fr);

        var player1 = new Player
        {
            Id = 5,
            FirstName = "Tom",
            LastName = "Lee",
            ShortName = "T.Lee",
            Picture = "https://example.com/tomlee.png",
            Data = new PlayerStats { Weight = 70000, Height = 175, Last = new[] { 1, 1, 0, 0, 1 } },
            Country = us,
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
            Country = fr,
            Sex = Sex.Female,
        };
        var player3 = new Player
        {
            Id = 3,
            FirstName = "Alice",
            LastName = "Johnson",
            ShortName = "A.Johnson",
            Picture = "https://example.com/alicejohnson.png",
            Country = fr,
            Data = new PlayerStats { Weight = 65000, Height = 165, Last = new[] { 1, 1, 0, 0, 1 } },
            Sex = Sex.Female
        };

        _dbContext.Players.AddRange(player1, player2, player3);

        _dbContext.SaveChanges();
    }

    // 160, 165, 175 => median = 165
    [Fact]
    public async Task GetSummaryStats_ReturnsCorrectStats_Odd()
    {
        // Arrange
        var handler = new GetSummaryStatsRequestHandler(_dbContext, _cachingService);
        var request = new GetSummaryStatsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.BestCountryByWinRatio);
        Assert.Equal("US", result.BestCountryByWinRatio.Code);

        var expectedImc = 23.39;
        Assert.Equal(result.PlayersAverageImc, expectedImc);

        var expectedMedianHeight = 165;
        Assert.Equal(expectedMedianHeight, result.PlayersHeightMean);
    }

    // 160, 165, 175, 180 => median = (165 + 175) / 2 = 170
    [Fact]
    public async Task GetSummaryStats_ReturnsCorrectStats_Even()
    {
        // Arrange
        var player4 = new Player
        {
            Id = 7,
            FirstName = "John",
            LastName = "Doe",
            ShortName = "J.Doe",
            Sex = Sex.Male,
            Picture = "https://example.com/johndoe.png",
            Data = new PlayerStats { Weight = 80000, Height = 180, Last = new[] { 1, 1, 0, 0, 1 } },
            CountryCode = "US",
        };

        _dbContext.Players.Add(player4);
        _dbContext.SaveChanges();

        var handler = new GetSummaryStatsRequestHandler(_dbContext, _cachingService);
        var request = new GetSummaryStatsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.BestCountryByWinRatio);
        Assert.Equal("US", result.BestCountryByWinRatio.Code);
        
        var expectedImc = 23.72;
        Assert.Equal(expectedImc, result.PlayersAverageImc);
        
        var expectedMedianHeight = 170;
        Assert.Equal(expectedMedianHeight, result.PlayersHeightMean);
    }
}
