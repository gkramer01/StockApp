using MediatR;
using StockApp.Application.Abstractions;
using StockApp.Shared.Result;

namespace StockApp.Application.Features.Products.Update
{
    public class UpdateProductHandler(IProductRepository repository) : IRequestHandler<UpdateProductCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            await repository.UpdateQuantityAsync(request.Id, request.Quantity, cancellationToken);
            return Result<Guid>.Ok(request.Id);
        }
    }
}
