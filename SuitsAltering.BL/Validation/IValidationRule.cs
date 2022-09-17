using FluentValidation;

namespace SuitsAltering.BL.Validation
{
    public interface IValidationRule<T>
    {
        Task ValidateAsync(ValidationContext<T> context);
    }
}
