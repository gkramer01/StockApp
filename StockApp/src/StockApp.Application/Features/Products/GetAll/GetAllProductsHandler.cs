using MediatR;
using StockApp.Application.Features.Products.Dtos;
using StockApp.Shared.Result;
using System.Data;
using Dapper;

namespace StockApp.Application.Features.Products.GetAll
{
    public class GetAllProductsHandler(IDbConnection connection) : IRequestHandler<GetAllProductsQuery, Result<List<ProductDto>>>
    {

        public async Task<Result<List<ProductDto>>> Handle(
       GetAllProductsQuery request,
       CancellationToken cancellationToken)
        {
            var sql = @"SELECT ""Id"", ""Name"", ""Quantity"" FROM products";

            var products = await connection.QueryAsync<ProductDto>(sql);

            return Result<List<ProductDto>>.Ok([.. products]);
        }
    }
}
