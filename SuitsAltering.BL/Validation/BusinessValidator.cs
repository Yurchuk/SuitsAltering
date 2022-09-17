using FluentValidation;
using FluentValidation.Results;

namespace SuitsAltering.BL.Validation
{
    internal class BusinessValidator<T> : AbstractValidator<T>
    {
        private readonly IEnumerable<IValidationRule<T>> _validationRules;

        public BusinessValidator(IEnumerable<IValidationRule<T>> validationRules)
        {
            _validationRules = validationRules;
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellation = default)
        {
            foreach (var rule in _validationRules)
            {
                await rule.ValidateAsync(context);
            }

            return await base.ValidateAsync(context, cancellation);
        }
    }
}
