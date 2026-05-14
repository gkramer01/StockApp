using StockApp.Application.Common.Messaging;
using StockApp.Shared.Result;

namespace StockApp.Application.Features.Products.Delete;

public record DeleteProductCommand(Guid Id) : ICommand<Result<Guid>>;


