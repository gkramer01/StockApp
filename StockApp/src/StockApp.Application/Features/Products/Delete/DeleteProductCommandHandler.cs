using MediatR;
using StockApp.Application.Abstractions;
using StockApp.Shared.Result;

namespace StockApp.Application.Features.Products.Delete;

public class DeleteProductCommandHandler(IProductRepository repository) : IRequestHandler<DeleteProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id, cancellationToken);
        return Result<Guid>.Ok(request.Id);
    }
}
