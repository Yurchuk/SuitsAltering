using System.Linq.Expressions;
using SuitsAltering.BL.Results;

namespace SuitsAltering.BL.Validation
{
    public interface IBusinessValidationBuilder<T>
    {
        IBusinessValidationBuilder<T> AddRule<TRule, TProperty>(Expression<Func<T, TProperty>> propertySelector, Error error)
            where TRule : IBusinessRule<TProperty>;

        IBusinessValidationBuilder<T> AddRule<TRule>(Error error)
            where TRule : IBusinessRule<T>;

        IBusinessValidationBuilder<T> AddRule<TRule, TProperty, TArgs>(Expression<Func<T, TProperty>> propertySelector, TArgs args, Error error)
            where TRule : IBusinessRule<TProperty, TArgs>;

        IBusinessValidationBuilder<T> AddRule<TRule, TArgs>(TArgs args, Error error)
            where TRule : IBusinessRule<T, TArgs>;

        IBusinessValidationBuilder<T> AddValidator<TValidator, TProperty>(Expression<Func<T, TProperty>> propertySelector)
            where TValidator : IBusinessValidator<TProperty>;

        IBusinessValidationBuilder<T> AddValidator<TValidator>()
            where TValidator : IBusinessValidator<T>;

        IBusinessValidator<T> Build();
    }
}
