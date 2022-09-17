using FluentValidation;
using SuitsAltering.Infrastructure.Extensions;

namespace SuitsAltering.API.Validation
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> IsIn<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, params TProperty[] items)
        {
            return ruleBuilder.Must(x => x.In(items));
        }
    }
}
