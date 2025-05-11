using Microsoft.EntityFrameworkCore;

namespace Infrastructures.DataAccess.Factories
{
    public sealed class UserDataContextFactory : IDbContextFactory<UserDataContext>
    {
        private readonly IDbContextFactory<UserDataContext> _factory;

        public UserDataContextFactory(IDbContextFactory<UserDataContext> factory)
        {
            _factory = factory;
        }

        public UserDataContext CreateDbContext()
        {
            return _factory.CreateDbContext();
        }
    }

}
