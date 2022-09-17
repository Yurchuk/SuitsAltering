using SuitsAltering.BL.Results;

namespace SuitsAltering.BL.Validation
{
    public interface IBusinessValidator<in T>
    {
        Task<Result> ValidateAsync(T instance);
    }

    public interface IBusinessValidator<in TModel, in TArgs>
    {
        Task<Result> ValidateAsync(TModel instance, TArgs args);
    }
}
