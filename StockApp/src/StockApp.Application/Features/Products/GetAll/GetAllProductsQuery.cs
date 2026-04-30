using MediatR;
using StockApp.Application.Features.Products.Dtos;
using StockApp.Shared.Result;

namespace StockApp.Application.Features.Products.GetAll
{
    public record GetAllProductsQuery() : IRequest<Result<List<ProductDto>>>;
}
