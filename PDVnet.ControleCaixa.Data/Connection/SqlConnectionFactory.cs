using Microsoft.Data.SqlClient;
using System.Configuration;

namespace PDVnet.ControleCaixa.Data.Connection
{
    public sealed class SqlConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory()
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"];

            if (connection is null || string.IsNullOrWhiteSpace(connection.ConnectionString))
            {
                throw new InvalidOperationException(
                    "A connection string 'DefaultConnection' não foi encontrada no App.config.");
            }

            _connectionString = connection.ConnectionString;
        }

        public SqlConnection Create()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
