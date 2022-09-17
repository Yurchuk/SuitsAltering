using Serilog;

namespace SuitsAltering.API.Common;

public class ErrorResponse
{
    public IEnumerable<ErrorModel> Errors { get; set; } = Enumerable.Empty<ErrorModel>();

    public IEnumerable<ErrorModel> Warnings { get; set; } = Enumerable.Empty<ErrorModel>();

    public ErrorResponse WithErrors(params ErrorModel[] errors)
    {
        Errors = Errors.Concat(errors);
        return this;
    }

    public ErrorResponse WithWarnings(params ErrorModel[] warnings)
    {
        Warnings = Warnings.Concat(warnings);
        return this;
    }

    public class ErrorModel
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string FieldName { get; set; }
    }
}