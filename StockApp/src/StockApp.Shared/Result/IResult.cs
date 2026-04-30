
using StockApp.Shared.Errors;

namespace StockApp.Shared.Result
{
    public interface IResult
    {
        bool Success { get; }
        List<Error> Errors { get; }
    }
}
