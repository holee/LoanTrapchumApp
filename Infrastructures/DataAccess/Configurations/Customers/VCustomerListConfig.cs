using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public class VCustomerListConfig : IEntityTypeConfiguration<VCustomerList>
    {
        public void Configure(EntityTypeBuilder<VCustomerList> builder)
        {
            builder.ToView("VCustomerList");
            builder.HasNoKey();
            builder.HasQueryFilter(x => x.IsActive == true);
        }
    }
}
