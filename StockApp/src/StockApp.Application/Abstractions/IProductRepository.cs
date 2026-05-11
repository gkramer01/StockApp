using StockApp.Domain.Entities;

namespace StockApp.Application.Abstractions;

public interface IProductRepository
{
    Task CreateAsync(Product product, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateQuantityAsync(Guid id, int quantity, CancellationToken cancellationToken);
}