using StockApp.Application.Features.Products.Dtos;
using StockApp.Domain.Entities;

namespace StockApp.Application.Abstractions;

public interface IProductRepository
{
    Task CreateAsync(Product product);
    Task DeleteAsync(Guid id);
}