using FluentValidation;
using Takwene.Application.DTOs.Artists;

namespace Takwene.Application.Validators
{
    public class CreateArtistRequestValidator : AbstractValidator<CreateArtistRequest>
    {
        public CreateArtistRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        }
    }
}
