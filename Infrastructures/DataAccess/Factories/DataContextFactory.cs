using Microsoft.EntityFrameworkCore;

namespace Infrastructures.DataAccess.Factories
{

    public sealed class DataContextFactory(IDbContextFactory<DataContext> factory) : IDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext()
        {
            return factory.CreateDbContext();
        }
    }
}
