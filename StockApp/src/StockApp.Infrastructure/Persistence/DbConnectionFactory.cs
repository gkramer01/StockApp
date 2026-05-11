using Npgsql;
using StockApp.Application.Abstractions;
using System.Data;

namespace StockApp.Infrastructure.Persistence
{
    public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
    {
        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken)
        {
            var connection = new NpgsqlConnection(connectionString);

            await connection.OpenAsync(cancellationToken); 

            return connection;
        }
    }
}
