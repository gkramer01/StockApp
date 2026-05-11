using System.Data;

namespace StockApp.Application.Abstractions
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken);
    }
}
