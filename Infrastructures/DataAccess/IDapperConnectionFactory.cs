using System.Data;

namespace Infrastructures.Data;

public interface IDapperConnectionFactory
{
    IDbConnection CreateConnection();
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}