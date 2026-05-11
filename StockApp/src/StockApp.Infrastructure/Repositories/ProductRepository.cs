using Dapper;
using StockApp.Application.Abstractions;
using StockApp.Domain.Entities;

namespace StockApp.Infrastructure.Repositories;

public class ProductRepository(IUnitOfWork uow) : IProductRepository
{
    private readonly IUnitOfWork _uow = uow;

    public async Task CreateAsync(Product product, CancellationToken cancellationToken)
    {
        var sql = @"INSERT INTO products (""Id"", ""Name"", ""Quantity"")
                VALUES (@Id, @Name, @Quantity)";

        var command = new CommandDefinition(
            sql,
            new { product.Id, product.Name, product.Quantity },
            transaction: _uow.Transaction,
            cancellationToken: cancellationToken
        );

        await _uow.Connection.ExecuteAsync(command);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var sql = @"DELETE FROM products WHERE ""Id"" = @Id";

        var command = new CommandDefinition(
            sql,
            new { Id = id },
            transaction: _uow.Transaction,
            cancellationToken: cancellationToken
        );

        await _uow.Connection.ExecuteAsync(command);
    }

    public async Task UpdateQuantityAsync(Guid id, int quantity, CancellationToken cancellationToken)
    {
        var sql = """UPDATE products SET "Quantity" = @Quantity WHERE "Id" = @Id""";

        var command = new CommandDefinition(
            sql,
            new
            {
                Id = id,
                Quantity = quantity
            },
            transaction: _uow.Transaction,
            cancellationToken: cancellationToken
        );

        await _uow.Connection.ExecuteAsync(command);
    }
}