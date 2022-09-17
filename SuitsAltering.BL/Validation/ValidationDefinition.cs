using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;

namespace SuitsAltering.BL.Validation
{
    internal class ValidationDefinition<T, TProperty> : IValidationRule<T>
    {
        private readonly IMapper _mapper;
        private readonly IBusinessValidator<TProperty> _validator;
        private readonly Expression<Func<T, TProperty>> _propertySelector;

        public ValidationDefinition(
            IMapper mapper,
            IBusinessValidator<TProperty> validator,
            Expression<Func<T, TProperty>> propertySelector)
        {
            _mapper = mapper;
            _validator = validator;
            _propertySelector = propertySelector;
        }

        public async Task ValidateAsync(ValidationContext<T> context)
        {
            var model = context.InstanceToValidate;
            var value = _propertySelector.Compile()(model);

            var validationResult = await _validator.ValidateAsync(value);
            if (validationResult.IsFailed)
            {
                var validationFailures = _mapper.Map<IEnumerable<ValidationFailure>>(validationResult.Errors);
                foreach (var failure in validationFailures)
                {
                    context.AddFailure(failure);
                }
            }
        }
    }
}
