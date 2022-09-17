namespace SuitsAltering.BL.Results;

public enum ResultStatus
{
    Success = 0,

    ValidationError = 1,

    NotFound = 2,

    Unauthorized = 3,

    Forbidden = 4,

    Failed = 100,
}