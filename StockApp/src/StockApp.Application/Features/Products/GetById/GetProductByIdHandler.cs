using Dapper;
using MediatR;
using StockApp.Application.Abstractions;
using StockApp.Application.Features.Products.Dtos;
using StockApp.Shared.Errors;
using StockApp.Shared.Result;
using System.Data;

namespace StockApp.Application.Features.Products.GetById
{
    public class GetProductByIdHandler(IDbConnectionFactory factory) : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = await factory.CreateConnectionAsync(cancellationToken);

            var sql = @"SELECT ""Id"", ""Name"", ""Quantity"" FROM products WHERE ""Id"" = @Id";

            var product = await connection.QueryFirstOrDefaultAsync<ProductDto>(sql, new { request.Id });

            if (product == null)
            {
                return Result<ProductDto>.Fail([
                    Error.NotFound("Produto não encontrado")
                ]);
            }

            return Result<ProductDto>.Ok(product);
        }
    }
}
