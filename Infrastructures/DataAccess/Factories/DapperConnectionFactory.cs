using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructures.DataAccess.Factories;

public class DapperConnectionFactory(IConfiguration config) : IDapperConnectionFactory
{
    public IDbConnection CreateConnection() => new SqlConnection(config.GetConnectionString("DefaultConnection"));
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(token);
        return connection;
    }
}