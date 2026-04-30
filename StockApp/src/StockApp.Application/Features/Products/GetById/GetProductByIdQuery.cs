using MediatR;
using StockApp.Application.Features.Products.Dtos;
using StockApp.Shared.Result;

namespace StockApp.Application.Features.Products.GetById
{
    public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;
}
