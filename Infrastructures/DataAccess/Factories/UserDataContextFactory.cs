using Microsoft.EntityFrameworkCore;

namespace Infrastructures.DataAccess.Factories
{
    public sealed class UserDataContextFactory(IDbContextFactory<UserDataContext> factory)
        : IDbContextFactory<UserDataContext>
    {
        public UserDataContext CreateDbContext()
        {
            return factory.CreateDbContext();
        }
    }

}
