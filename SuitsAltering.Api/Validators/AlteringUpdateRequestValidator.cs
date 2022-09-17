using FluentValidation;
using SuitsAltering.API.Models;
using SuitsAltering.API.Validation;
using SuitsAltering.BL.Errors;
using SuitsAltering.DAL.Enums;

namespace SuitsAltering.API.Validators;

public class AlteringUpdateRequestValidator : AbstractValidator<UpdateAlteringRequest>
{
    public AlteringUpdateRequestValidator()
    {
        RuleFor(x => x.AlteringStatus).IsIn(AlteringStatus.Started, AlteringStatus.Done).WithError(AlteringErrors.CanBeUpdatedOnlyToStatusStartedOrDone);
    }
}