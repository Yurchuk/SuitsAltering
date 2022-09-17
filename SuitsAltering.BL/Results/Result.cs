namespace SuitsAltering.BL.Results;

public partial class Result
{
    public Result(ResultStatus status, IEnumerable<Error> errors)
        : this(status)
    {
        Errors = errors;
    }

    public Result(ResultStatus status, Error error)
        : this(status)
    {
        Errors = new List<Error> { error };
    }

    protected Result(ResultStatus status)
    {
        Status = status;
    }

    public ResultStatus Status { get; }

    public IEnumerable<Error> Errors { get; private set; }

    public IEnumerable<Error> Warnings { get; set; } = Enumerable.Empty<Error>();

    public bool IsSuccess => Status == ResultStatus.Success;

    public bool IsFailed => Status != ResultStatus.Success;

    public Result WithWarnings(params Error[] warnings)
    {
        Warnings = Warnings.Concat(warnings);
        return this;
    }

    public Result WithErrors(params Error[] errors)
    {
        Errors = Errors.Concat(errors);
        return this;
    }
}

public class Result<T> : Result
{
    private readonly T _value;

    public Result(ResultStatus status, T value)
        : base(status)
    {
        _value = value;
    }

    public Result(ResultStatus status, Error error)
        : base(status, error)
    {
    }

    public Result(ResultStatus status, IEnumerable<Error> errors)
        : base(status, errors)
    {
    }

    public T Value
        => IsSuccess
            ? _value
            : throw new InvalidOperationException("A failed result has no value.");

    public new Result<T> WithWarnings(params Error[] warnings)
    {
        Warnings = Warnings.Concat(warnings);
        return this;
    }
}

public partial class Result
    {
        public static Result Success()
        {
            return new Result(ResultStatus.Success);
        }

        public static Task<Result> SuccessTask()
        {
            return Task.FromResult(new Result(ResultStatus.Success));
        }

        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(ResultStatus.Success, value);
        }

        public static Task<Result<T>> SuccessTask<T>(T value)
        {
            return Task.FromResult(Success(value));
        }

        public static Result ValidationError(Error error)
        {
            return new Result(ResultStatus.ValidationError, error);
        }

        public static Task<Result> ValidationErrorTask(Error error)
        {
            return Task.FromResult(ValidationError(error));
        }

        public static Result ValidationError(string code, string message)
        {
            return new Result(ResultStatus.ValidationError, new Error(code, message));
        }

        public static Result ValidationError(IEnumerable<Error> errors)
        {
            return new Result(ResultStatus.ValidationError, errors);
        }

        public static Task<Result> ValidationErrorTask(IEnumerable<Error> errors)
        {
            return Task.FromResult(ValidationError(errors));
        }

        public static Result<T> ValidationError<T>(Error error)
        {
            return new Result<T>(ResultStatus.ValidationError, error);
        }

        public static Result<T> ValidationError<T>(string code, string message)
        {
            return new Result<T>(ResultStatus.ValidationError, new Error(code, message));
        }

        public static Result<T> ValidationError<T>(IEnumerable<Error> errors)
        {
            return new Result<T>(ResultStatus.ValidationError, errors);
        }

        public static Result NotFound(Error error)
        {
            return new Result(ResultStatus.NotFound, error);
        }

        public static Result<T> NotFound<T>(Error error)
        {
            return new Result<T>(ResultStatus.NotFound, error);
        }

        public static Result Failure(Error error)
        {
            return new Result(ResultStatus.Failed, error);
        }

        public static Result<T> Failure<T>(Error error)
        {
            return new Result<T>(ResultStatus.Failed, error);
        }

        public static Result Unauthorized(Error error)
        {
            return new Result(ResultStatus.Unauthorized, error);
        }

        public static Result<T> Unauthorized<T>(Error error)
        {
            return new Result<T>(ResultStatus.Unauthorized, error);
        }

        public static Result Unauthorized(IEnumerable<Error> errors)
        {
            return new Result(ResultStatus.Unauthorized, errors);
        }

        public static Result<T> Unauthorized<T>(IEnumerable<Error> errors)
        {
            return new Result<T>(ResultStatus.Unauthorized, errors);
        }

        public static Result Forbidden(IEnumerable<Error> errors)
        {
            return new Result(ResultStatus.Forbidden, errors);
        }

        public static Result FromResult<TR>(Result<TR> result)
        {
            return new Result(result.Status, result.Errors).WithWarnings(result.Warnings.ToArray());
        }

        public static Result<T> FromResult<T>(Result result, T value)
        {
            return new Result<T>(result.Status, value) { Errors = result.Errors, Warnings = result.Warnings };
        }

        public static Result<T> FailureFromResult<T>(Result result)
        {
            return new Result<T>(ResultStatus.Failed, result.Errors);
        }

        public static Result<T> FromErrorResult<T>(Result result)
        {
            return new Result<T>(result.Status, result.Errors).WithWarnings(result.Warnings.ToArray());
        }
    }