using Applications.Responses.ViewModels.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public sealed class VCustomerIncomeConfig : IBaseEntityTypeConfigurations<VCustomerIncomes>
    {
        public void Configure(EntityTypeBuilder<VCustomerIncomes> builder)
        {
            builder.ToView("VCustomerIncomes");
            builder.HasNoKey();
            builder.Property(x => x.IncomeAmount).HasPrecision(18, 3);
        }
    }
}
