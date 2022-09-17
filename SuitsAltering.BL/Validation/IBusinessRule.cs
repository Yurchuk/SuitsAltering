namespace SuitsAltering.BL.Validation
{
    public interface IBusinessRule<in T>
    {
        Task<bool> IsValidAsync(T model);
    }

    public interface IBusinessRule<in TModel, in TArgs>
    {
        Task<bool> IsValidAsync(TModel model, TArgs args);
    }
}
