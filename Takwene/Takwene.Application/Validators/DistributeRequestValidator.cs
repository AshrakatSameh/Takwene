using System.Linq;
using FluentValidation;
using Takwene.Application.DTOs.Tracks;

namespace Takwene.Application.Validators
{
    public class DistributeRequestValidator : AbstractValidator<DistributeRequest>
    {
        public DistributeRequestValidator()
        {
            RuleFor(x => x.DspIds)
                .NotNull().NotEmpty()
                .WithMessage("At least one DSP id is required.");
            RuleForEach(x => x.DspIds).GreaterThan(0);
            RuleFor(x => x.DspIds)
                .Must(ids => ids == null || ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate DSP ids are not allowed.");
        }
    }
}
