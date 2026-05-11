using System.Data;

namespace StockApp.Application.Abstractions
{
    public interface IUnitOfWork
    {
        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        Task BeginTransactionAsync(CancellationToken cancellationToken);

        Task CommitAsync(CancellationToken cancellationToken);

        Task RollbackAsync(CancellationToken cancellationToken);
    }
}
