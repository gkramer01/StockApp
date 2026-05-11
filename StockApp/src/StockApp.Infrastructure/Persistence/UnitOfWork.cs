using StockApp.Application.Abstractions;
using System.Data;

namespace StockApp.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnectionFactory _factory;

        private IDbConnection? _connection;
        private IDbTransaction? _transaction;

        public UnitOfWork(IDbConnectionFactory factory) => _factory = factory;

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            _connection = await _factory.CreateConnectionAsync(cancellationToken);

            _transaction = _connection.BeginTransaction();
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            _transaction?.Commit();

            Dispose();

            return Task.CompletedTask;
        }

        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            _transaction?.Rollback();

            Dispose();

            return Task.CompletedTask;
        }

        public IDbConnection Connection =>
            _connection ?? throw new InvalidOperationException("Transaction not started");

        public IDbTransaction Transaction =>
            _transaction ?? throw new InvalidOperationException("Transaction not started");

        private void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}