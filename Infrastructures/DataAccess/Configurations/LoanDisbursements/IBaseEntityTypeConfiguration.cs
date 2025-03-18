using Microsoft.EntityFrameworkCore;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public interface IBaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
        where T : class
    {

    }
}
