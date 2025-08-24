using TennisCleanArchi.Application.Players.ListPlayers;

namespace TennisCleanArchi.Application.Tests.Players.ListPlayers;

public class ListPlayersRequestValidatorTests : BaseTests
{
    [Fact]
    public async Task Validate_WithPageSizeOutOfBounds_ReturnsInvalidResult()
    {
        var requestHigh = new ListPlayersRequest
        {
            PageNumber = -1,
            PageSize = 100000 // Invalid, too high
        };

        var requestLow = new ListPlayersRequest
        {
            PageNumber = 1,
            PageSize = -10 // Invalid, too low
        };

        var validator = new ListPlayersRequestValidator();

        var result1 = await validator.ValidateAsync(requestHigh);

        Assert.False(result1.IsValid);
        Assert.Contains(result1.Errors, e => e.PropertyName == "PageSize");
        Assert.Contains(result1.Errors, e => e.PropertyName == "PageNumber");

        var result2 = await validator.ValidateAsync(requestLow);
        Assert.False(result2.IsValid);
        Assert.Contains(result2.Errors, e => e.PropertyName == "PageSize");
    }
}