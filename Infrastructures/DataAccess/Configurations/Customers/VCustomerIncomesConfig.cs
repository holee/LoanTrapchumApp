using Applications.Responses.ViewModels.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public sealed class VCustomerIncomesConfig : IBaseEntityTypeConfigurations<VCustomerIncomes>
    {
        public void Configure(EntityTypeBuilder<VCustomerIncomes> builder)
        { 
            builder.ToView("VCustomerIncomes");
            builder.HasNoKey();
        }
    }
}
