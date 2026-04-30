using Dapper;
using StockApp.Application.Abstractions;
using StockApp.Application.Features.Products.Dtos;
using StockApp.Domain.Entities;
using System.Data;

namespace StockApp.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnection _connection;

    public ProductRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task CreateAsync(Product product)
    {
        var sql = @"INSERT INTO products (""Id"", ""Name"", ""Quantity"")
                VALUES (@Id, @Name, @Quantity)";

        await _connection.ExecuteAsync(sql, product);
    }

    public async Task DeleteAsync(Guid id)
    {
        var sql = @"DELETE FROM products WHERE ""Id"" = @Id";
        await _connection.ExecuteAsync(sql, new { id });
    }
}