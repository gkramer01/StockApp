using MediatR;
using StockApp.Domain.Entities;
using StockApp.Application.Abstractions;
using StockApp.Shared.Result;

namespace StockApp.Application.Features.Products.Create;

public class CreateProductHandler
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _repository;

    public CreateProductHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = new Product(
            request.Name,
            request.Quantity
        );

        Console.WriteLine("Tentando criar produto.");
        try
        {
            await _repository.CreateAsync(product, cancellationToken);
            Console.WriteLine("Produto criado com sucesso.");
        }
        catch (Exception ex)
        {

            throw new Exception("Erro ao criar produto.", ex);
        }


        return Result<Guid>.Ok(product.Id);
    }
}
