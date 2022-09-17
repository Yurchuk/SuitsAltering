using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using SuitsAltering.BL.Results;

namespace SuitsAltering.BL.Validation
{
    internal class RuleDefinition<T, TProperty> : IValidationRule<T>
    {
        private readonly IBusinessRule<TProperty> _businessRule;
        private readonly Error _error;
        private readonly Expression<Func<T, TProperty>> _propertySelector;

        public RuleDefinition(
            IBusinessRule<TProperty> businessRule,
            Error error,
            Expression<Func<T, TProperty>> propertySelector)
        {
            _businessRule = businessRule;
            _error = error;
            _propertySelector = propertySelector;
        }

        public async Task ValidateAsync(ValidationContext<T> context)
        {
            var model = context.InstanceToValidate;
            var value = _propertySelector.Compile()(model);

            var isValid = await _businessRule.IsValidAsync(value);
            if (!isValid)
            {
                context.AddFailure(new ValidationFailure(_error.FieldName, _error.Message)
                {
                    ErrorCode = _error.Code,
                    CustomState = _error.Parameters,
                    PropertyName = _error.FieldName ?? (_propertySelector.Body as MemberExpression)?.Member.Name,
                });
            }
        }
    }
}
