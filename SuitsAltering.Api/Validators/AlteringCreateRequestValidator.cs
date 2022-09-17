using FluentValidation;
using SuitsAltering.API.Models;
using SuitsAltering.API.Validation;
using SuitsAltering.BL.Errors;

namespace SuitsAltering.API.Validators;

public class AlteringCreateRequestValidator : AbstractValidator<CreateAlteringRequest>
{
    public AlteringCreateRequestValidator()
    {
        RuleFor(x => x.ClothingType).IsInEnum().WithError(AlteringErrors.InvalidClothingType);
        RuleFor(x => x.RightAdjustment).InclusiveBetween(-5, 5).WithError(AlteringErrors.InvalidAlteringLengthRange);
        RuleFor(x => x.LeftAdjustment).InclusiveBetween(-5, 5).LessThanOrEqualTo(5).WithError(AlteringErrors.InvalidAlteringLengthRange);

        RuleFor(x => x.RightAdjustment).NotEqual(0).When(x => x.LeftAdjustment == 0).WithError(AlteringErrors.BothAdjustmentsCannotBeZero);
        RuleFor(x => x.LeftAdjustment).NotEqual(0).When(x => x.RightAdjustment == 0).WithError(AlteringErrors.BothAdjustmentsCannotBeZero);

    }
}