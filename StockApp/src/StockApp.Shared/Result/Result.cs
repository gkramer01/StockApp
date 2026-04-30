using StockApp.Shared.Errors;

namespace StockApp.Shared.Result;

public class Result<T> : IResult
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public List<Error> Errors { get; private set; } = [];

    public static Result<T> Ok(T data)
        => new() { Success = true, Data = data };

    public static Result<T> Fail(List<Error> errors)
        => new() { Success = false, Errors = errors };

    public static Result<T> Fail(Error error)
    => new() { Success = false, Errors = [error] };
}