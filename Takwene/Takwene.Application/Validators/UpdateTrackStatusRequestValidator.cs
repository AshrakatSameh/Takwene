using FluentValidation;
using Takwene.Application.DTOs.Tracks;

namespace Takwene.Application.Validators
{
    public class UpdateTrackStatusRequestValidator : AbstractValidator<UpdateTrackStatusRequest>
    {
        public UpdateTrackStatusRequestValidator()
        {
            RuleFor(x => x.Status).IsInEnum()
                .WithMessage("Status must be one of: Draft, Submitted, Distributed.");
        }
    }
}
