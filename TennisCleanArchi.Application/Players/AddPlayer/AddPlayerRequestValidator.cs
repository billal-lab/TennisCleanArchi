using FluentValidation;
using TennisCleanArchi.Shared;

namespace TennisCleanArchi.Application.Players.AddPlayer;

public class AddPlayerRequestValidator : AbstractValidator<AddPlayerRequest>
{
    public AddPlayerRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(255);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(255);

        RuleFor(x => x.ShortName)
            .NotEmpty().WithMessage("Short name is required.")
            .MaximumLength(255);

        RuleFor(x => x.Sex)
            .NotEmpty().WithMessage("Sex is required.")
            .Must(Sex.IsValidValue).WithMessage("Sex must be either 'M' or 'F'.");

        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("Country code is required.")
            .MaximumLength(3);

        RuleFor(x => x.Picture)
            .NotEmpty().WithMessage("Picture is required.")
            .MaximumLength(255)
            .Must(link => Uri.TryCreate(link, UriKind.Absolute, out var uri) && (uri.Scheme == "http" || uri.Scheme == "https"))
            .WithMessage("Picture must be a valid URL.")
            .Must(link => link.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                        || link.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                        || (link.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
                        || link.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Picture must have a .png, .jpg, webp or .jpeg extension.");

        RuleFor(x => x.Data)
            .NotNull().WithMessage("Data is required.");

        When(x => x.Data != null, () =>
        {
            RuleFor(x => x.Data.Rank)
                .GreaterThan(0).WithMessage("Rank must be greater than 0.");
            RuleFor(x => x.Data.Points)
                .GreaterThanOrEqualTo(0).WithMessage("Points must be 0 or greater.");
            RuleFor(x => x.Data.Weight)
                .InclusiveBetween(1, 200000).WithMessage("Weight must be between 1 and 200000.");
            RuleFor(x => x.Data.Height)
                .InclusiveBetween(1, 300).WithMessage("Height must be between 1 and 300.");
            RuleFor(x => x.Data.Age)
                .InclusiveBetween(1, 120).WithMessage("Age must be between 1 and 120.");
            RuleFor(x => x.Data.Last).NotNull()
                .Must(arr => arr.Length <= 20).WithMessage("Last match results array must have at most 10000 elements.");
            RuleForEach(x => x.Data.Last)
                .Must(result => result == 0 || result == 1)
                .WithMessage("Each element in Last match results must be either 0 (loss) or 1 (win).");
        });
    }
}
