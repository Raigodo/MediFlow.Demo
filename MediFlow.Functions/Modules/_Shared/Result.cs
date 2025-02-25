namespace MediFlow.Functions.Modules._Shared;

public readonly record struct Result<TResult, TErrors>
    where TErrors : struct, Enum
{
    private Result(TResult value)
    {
        Value = value;
        IsSuccess = true;
        ErrorCode = default;
    }

    private Result(TErrors errorCode, Dictionary<string, string[]>? errors = null)
    {
        Value = default;
        IsSuccess = false;
        ErrorCode = errorCode;
        Errors = errors;
    }

    public readonly TResult? Value;
    public readonly bool IsSuccess;
    public readonly TErrors ErrorCode;
    public readonly Dictionary<string, string[]>? Errors;

    public static Result<TResult, TErrors> Success(TResult value) => new(value);
    public static Result<TResult, TErrors> Fail(
        TErrors errorCode,
        Dictionary<string, string[]>? errors)
    {
        return new(errorCode, errors);
    }


    public static implicit operator Result<TResult, TErrors>(TResult value) => Success(value);
    public static implicit operator Result<TResult, TErrors>(TErrors value) => Fail(value, null);
}