using FluentValidation;
using SuitsAltering.BL.Results;

namespace SuitsAltering.API.Validation
{
    public static class RuleBuilderOptionsExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilderOptions, Error error)
        {
            return ruleBuilderOptions.WithErrorCode(error.Code).WithMessage(error.Message).WithState(x => error.Parameters);
        }
    }
}
