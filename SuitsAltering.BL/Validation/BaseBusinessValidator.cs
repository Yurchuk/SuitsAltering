using System.Linq.Expressions;
using SuitsAltering.BL.Results;

namespace SuitsAltering.BL.Validation
{
    public abstract class BaseBusinessValidator<T> : IBusinessValidator<T>
    {
        private IBusinessValidationBuilder<T> _builder;

        protected BaseBusinessValidator(IBusinessValidationBuilder<T> builder)
        {
            _builder = builder;
        }

        public virtual async Task<Result> ValidateAsync(T instance)
        {
            var validator = _builder.Build();

            return await validator.ValidateAsync(instance);
        }

        protected void AddRule<TRule, TProperty>(Expression<Func<T, TProperty>> propertySelector, Error error)
            where TRule : IBusinessRule<TProperty>
        {
            _builder = _builder.AddRule<TRule, TProperty>(propertySelector, error);
        }

        protected void AddRule<TRule>(Error error)
            where TRule : IBusinessRule<T>
        {
            _builder = _builder.AddRule<TRule>(error);
        }

        protected void AddRule<TRule, TProperty, TArgs>(
            Expression<Func<T, TProperty>> propertySelector, TArgs args, Error error)
            where TRule : IBusinessRule<TProperty, TArgs>
        {
            _builder = _builder.AddRule<TRule, TProperty, TArgs>(propertySelector, args, error);
        }

        protected void AddRule<TRule, TArgs>(TArgs args, Error error)
            where TRule : IBusinessRule<T, TArgs>
        {
            _builder = _builder.AddRule<TRule, TArgs>(args, error);
        }

        protected void AddValidator<TValidator, TProperty>(Expression<Func<T, TProperty>> propertySelector)
            where TValidator : IBusinessValidator<TProperty>
        {
            _builder = _builder.AddValidator<TValidator, TProperty>(propertySelector);
        }

        protected void AddValidator<TValidator>()
            where TValidator : IBusinessValidator<T>
        {
            _builder = _builder.AddValidator<TValidator>();
        }
    }
}
