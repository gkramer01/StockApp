using StockApp.Application.Common.Messaging;
using StockApp.Shared.Result;

namespace StockApp.Application.Features.Products.Update
{
    public record UpdateProductCommand(Guid Id, string Name, int Quantity) : ICommand<Result<Guid>>;
}
