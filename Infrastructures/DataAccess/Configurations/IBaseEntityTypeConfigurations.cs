using Microsoft.EntityFrameworkCore;

namespace Infrastructures.DataAccess.Configurations
{
    public interface IBaseEntityTypeConfigurations<T> : IEntityTypeConfiguration<T>
        where T : class
    {

    }
}
