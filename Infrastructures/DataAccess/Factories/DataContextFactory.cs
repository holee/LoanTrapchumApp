using Microsoft.EntityFrameworkCore;

namespace Infrastructures.DataAccess.Factories
{

    public sealed class DataContextFactory : IDbContextFactory<DataContext>
    {
        private readonly IDbContextFactory<DataContext> _factory;

        public DataContextFactory(IDbContextFactory<DataContext> factory)
        {
            _factory = factory;
        }

        public DataContext CreateDbContext()
        {
            return _factory.CreateDbContext();
        }
    }
}
