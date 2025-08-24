using TennisCleanArchi.Application.Countries.ListCountries;

namespace TennisCleanArchi.Application.Tests.Countries.ListCountries;

public class ListCountriesRequestValidatorTests
{
    [Fact]
    public async Task Validate_WithPageSizeOutOfBounds_ReturnsInvalidResult()
    {
        var requestHigh = new ListCountriesRequest
        {
            PageNumber = -1,
            PageSize = 100000 // Invalid, too high
        };

        var requestLow = new ListCountriesRequest
        {
            PageNumber = 1,
            PageSize = -10 // Invalid, too low
        };

        var validator = new ListCountriesRequestValidator();

        var result1 = await validator.ValidateAsync(requestHigh);

        Assert.False(result1.IsValid);
        Assert.Contains(result1.Errors, e => e.PropertyName == "PageSize");
        Assert.Contains(result1.Errors, e => e.PropertyName == "PageNumber");

        var result2 = await validator.ValidateAsync(requestLow);
        Assert.False(result2.IsValid);
        Assert.Contains(result2.Errors, e => e.PropertyName == "PageSize");
    }
}
