using StockApp.Domain.Entities;

namespace StockApp.Application.Abstractions;

public interface IProductRepository
{
    Task CreateAsync(Product product, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, string name, int quantity, CancellationToken cancellationToken);
}