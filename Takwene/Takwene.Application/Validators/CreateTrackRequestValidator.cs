using FluentValidation;
using Takwene.Application.DTOs.Tracks;

namespace Takwene.Application.Validators
{
    public class CreateTrackRequestValidator : AbstractValidator<CreateTrackRequest>
    {
        public CreateTrackRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
            RuleFor(x => x.ArtistId).GreaterThan(0);
            RuleFor(x => x.Isrc)
                .NotEmpty()
                .Matches("^[A-Za-z0-9]{12}$")
                .WithMessage("ISRC must be exactly 12 alphanumeric characters (e.g. USRC17607839).");
            RuleFor(x => x.Genre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ReleaseDate)
                .NotEqual(default(System.DateOnly))
                .WithMessage("ReleaseDate is required.");
        }
    }
}
