namespace StockApp.Shared.Errors;

public class Error
{
    public string Code { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;

    public static Error Validation(string message)
        => new() { Code = "VALIDATION_ERROR", Message = message };

    public static Error NotFound(string message)
        => new() { Code = "NOT_FOUND", Message = message };
}
