using Microsoft.EntityFrameworkCore;

namespace Infrastructures.DataAccess.Factories;

public class DbContextFactory<TContext>(Func<TContext> contextCreator) : IDbContextFactory<TContext>
    where TContext : DbContext
{
    private readonly Func<TContext> _contextCreator = contextCreator ?? throw new ArgumentNullException(nameof(contextCreator));
    public TContext CreateDbContext() => _contextCreator();
}