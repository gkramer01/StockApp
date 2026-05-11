using MediatR;
using StockApp.Application.Abstractions;
using StockApp.Application.Common.Messaging;

namespace StockApp.Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    private readonly IUnitOfWork _uow;

    public TransactionBehavior(IUnitOfWork uow) => _uow = uow;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync(cancellationToken);

        try
        {
            var response = await next(cancellationToken);

            await _uow.CommitAsync(cancellationToken);

            return response;
        }
        catch
        {
            await _uow.RollbackAsync(cancellationToken);

            throw;
        }
    }
}
