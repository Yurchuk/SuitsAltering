using SuitsAltering.API.Common;

namespace SuitsAltering.API.Models;

public class SuccessResponse
{
    public IEnumerable<ErrorResponse.ErrorModel> Warnings { get; set; } = Enumerable.Empty<ErrorResponse.ErrorModel>();

    public SuccessResponse WithWarnings(params ErrorResponse.ErrorModel[] warnings)
    {
        Warnings = Warnings.Concat(warnings);
        return this;
    }
}

public class SuccessResponse<T>
{
    public T Result { get; set; }

    public IEnumerable<ErrorResponse.ErrorModel> Warnings { get; set; } = Enumerable.Empty<ErrorResponse.ErrorModel>();

    public SuccessResponse<T> WithWarnings(params ErrorResponse.ErrorModel[] warnings)
    {
        Warnings = Warnings.Concat(warnings);
        return this;
    }
}