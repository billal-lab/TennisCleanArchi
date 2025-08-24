using FluentValidation;
using TennisCleanArchi.Application.Players.AddPlayer;

namespace TennisCleanArchi.Application.Tests.Players.AddPlayer;

public class AddPlayerRequestValidatorTests : BaseTests
{
    [Fact]
    public async Task Validate_WrongSex_ShouldThrowValidationException()
    {
        // Arrange: Create a request with "V" which is not valid (should be "M" or "F")
        var request = new AddPlayerRequest
        {
            FirstName = "Roger",
            LastName = "Federer",
            CountryCode = "USA",
            Sex = "V",
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

        var validator = new AddPlayerRequestValidator();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(async () => await validator.ValidateAndThrowAsync(request));
    }

    [Fact]
    public async Task Validate_WithInvalidRequest_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = new AddPlayerRequest
        {
            FirstName = "Roger",
            LastName = "Federer",
            CountryCode = "USAZ", // Invalid code
            Sex = "V",            // Invalid sex
            ShortName = "R.Fed",
            Picture = "http://example.com/picture.mp4", // Invalid extension
            Data = new Domain.PlayerStats
            {
                Age = 40,
                Height = 0,   // Invalid (must be > 0)
                Weight = 0,   // Invalid (must be > 0)
                Points = 0,   // Likely invalid
                Rank = -10,   // Invalid (must be > 0)
                Last = [1, 1, 0, 1, 5] // Invalid (5 is not 0 or 1)
            }
        };
        var validator = new AddPlayerRequestValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CountryCode");
        Assert.Contains(result.Errors, e => e.PropertyName == "Sex");
        Assert.Contains(result.Errors, e => e.PropertyName == "Picture");
        Assert.Contains(result.Errors, e => e.PropertyName == "Data.Rank");
        Assert.Contains(result.Errors, e => e.PropertyName == "Data.Weight");
        Assert.Contains(result.Errors, e => e.PropertyName == "Data.Height");
        Assert.Contains(result.Errors, e => e.PropertyName == "Data.Last[4]");
    }

    [Fact]
    public async Task Validate_ValidRequest_ShouldPassValidation()
    {
        // Arrage
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

        var validator = new AddPlayerRequestValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        Assert.True(result.IsValid);
    }
}
