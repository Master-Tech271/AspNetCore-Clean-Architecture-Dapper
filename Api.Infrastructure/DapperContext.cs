using System.Data;
using System.Data.SqlClient;

namespace Api.Infrastructure
{
    public class DapperContext : IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection = default!;

        public DapperContext(string? connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _connection = new SqlConnection(_connectionString);
        }

        public IDbConnection GetOpenConnection()
        {
            // Open the connection if it is not already open
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }

        public void Dispose()
        {
            // Dispose of the connection if it's not null
            _connection?.Dispose();
        }
    }
}
