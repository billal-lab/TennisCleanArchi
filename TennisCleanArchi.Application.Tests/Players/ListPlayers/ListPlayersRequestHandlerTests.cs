using TennisCleanArchi.Application.Common.Data;
using TennisCleanArchi.Application.Players.ListPlayers;
using TennisCleanArchi.Domain;
using TennisCleanArchi.Shared;

namespace TennisCleanArchi.Application.Tests.Players.ListPlayers;

public class ListPlayersRequestHandlerTests : BaseTests
{
    private readonly IApplicationDbContext _dbContext;

    public ListPlayersRequestHandlerTests()
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
            Data = new PlayerStats { Rank = 2, Weight = 70000, Height = 175, Last = [1, 1, 0, 0, 1] },
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
            Data = new PlayerStats { Rank = 5, Weight = 60000, Height = 160, Last = [1, 1, 0, 0, 1] },
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
            Data = new PlayerStats { Rank = 1,  Weight = 60000, Height = 160, Last = [1, 1, 0, 0, 1] },
            Sex = Sex.Female
        };

        _dbContext.Players.AddRange(player1, player2, player3);

        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Handle_WithDefaultPageRequest_ReturnsFirstPageWithDefaultPageSize()
    {
        // Arrange
        var handler = new ListPlayersRequestHandler(_dbContext);
        var request = new ListPlayersRequest
        {
            PageNumber = 1,
            PageSize = 2
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(3, result.Total);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(3, result.Items[0].Id); // Ordered by Rank ascending
    }

    [Fact]
    public async Task Handle_WithSecondPageRequest_ReturnsSecondPageWithCustomPageSize()
    {
        // Arrange
        var handler = new ListPlayersRequestHandler(_dbContext);
        var request = new ListPlayersRequest
        {
            PageNumber = 2,
            PageSize = 2
        };
        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.PageNumber);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(3, result.Total);
        Assert.Single(result.Items);
        Assert.Equal(6, result.Items[0].Id); // Ordered by Rank ascending
    }

    [Fact]
    public async Task Handle_WithPageNumberExceedingTotalPages_ReturnsEmptyPage()
    {
        // Arrange
        var handler = new ListPlayersRequestHandler(_dbContext);
        var request = new ListPlayersRequest
        {
            PageNumber = 4,
            PageSize = 2
        };
        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.PageNumber);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(3, result.Total);
        Assert.Empty(result.Items);
    }
}
