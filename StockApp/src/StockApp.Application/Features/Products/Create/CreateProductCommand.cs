namespace StockApp.Application.Features.Products.Create;

using MediatR;
using StockApp.Shared.Result;

public record CreateProductCommand(string Name, int Quantity = 0) : IRequest<Result<Guid>>;

