using StockApp.Application.Common.Messaging;
using StockApp.Shared.Result;

namespace StockApp.Application.Features.Products.Update
{
    public record UpdateProductCommand(Guid Id, int Quantity) : ICommand<Result<Guid>>;
}
