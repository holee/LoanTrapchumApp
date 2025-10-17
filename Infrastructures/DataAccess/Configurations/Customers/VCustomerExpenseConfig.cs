using Applications.Responses.ViewModels.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public sealed class VCustomerExpenseConfig : IBaseEntityTypeConfigurations<VCustomerExpenses>
    {
        public void Configure(EntityTypeBuilder<VCustomerExpenses> builder)
        {
            builder.ToView("VCustomerExpenses");
            builder.HasNoKey();
            builder.Property(x => x.ExpendAmount).HasPrecision(18, 3);
        }
    }
}
