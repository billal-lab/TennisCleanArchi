using TennisCleanArchi.Application.Common.Data;
using TennisCleanArchi.Application.Countries.ListCountries;

namespace TennisCleanArchi.Application.Tests.Countries.ListCountries;

public class ListCountriesRequestHandlerTests : BaseTests
{
    private readonly IApplicationDbContext _dbContext;

    public ListCountriesRequestHandlerTests()
    {
        _dbContext = Fixture.DbContext;
        SeedData();
    }

    private void SeedData()
    {
        // Seed countries
        _dbContext.Countries.Add(new Domain.Country { Code = "FRA", Picture = "http://example.com/fr.png" });
        _dbContext.Countries.Add(new Domain.Country { Code = "USA", Picture = "http://example.com/us.png" });
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Handle_WithCountries_ReturnsAllCountries()
    {
        var handler = new ListCountriesRequestHandler(_dbContext);
        var request = new ListCountriesRequest();

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Total);
        Assert.Equal(2, result.Items.Count);
        Assert.Contains(result.Items, c => c.Code == "FRA");
        Assert.Contains(result.Items, c => c.Code == "USA");
    }

    [Fact]
    public async Task Handle_WithNoCountries_ReturnsEmptyList()
    {
        // Clear countries
        _dbContext.Countries.RemoveRange(_dbContext.Countries);
        _dbContext.SaveChanges();

        var handler = new ListCountriesRequestHandler(_dbContext);
        var request = new ListCountriesRequest();

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.Total);
    }
}
