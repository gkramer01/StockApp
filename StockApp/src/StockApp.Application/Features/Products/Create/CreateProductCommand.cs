using StockApp.Application.Common.Messaging;
using StockApp.Shared.Result;

namespace StockApp.Application.Features.Products.Create;

public record CreateProductCommand(string Name, int Quantity = 0) : ICommand<Result<Guid>>;

