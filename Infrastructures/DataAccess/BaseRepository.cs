using Microsoft.EntityFrameworkCore;

namespace Infrastructures.DataAccess
{
    public abstract class BaseRepository<TContext> where TContext : DbContext
    {
        private readonly IDbContextFactory<TContext> _contextFactory;

        protected BaseRepository(IDbContextFactory<TContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        protected async Task<TResult> WithDbContextAsync<TResult>(Func<TContext, Task<TResult>> operation)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await operation(context);
        }

        protected async Task WithDbContextAsync(Func<TContext, Task> operation)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            await operation(context);
        }
    }
}
