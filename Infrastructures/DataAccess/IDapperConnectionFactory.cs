using System.Data;

namespace Infrastructures.DataAccess;

public interface IDapperConnectionFactory
{
    IDbConnection CreateConnection();
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}