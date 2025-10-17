using Applications.Responses.ViewModels.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public sealed class VCustomerActiviesConfig : IBaseEntityTypeConfigurations<VCustomerActivities>
    {
        public void Configure(EntityTypeBuilder<VCustomerActivities> builder)
        {
            builder.ToView("VCustomerActivities");
            builder.HasKey(x => new { x.ActivityId, x.CustomerRefId });
        }
    }
}
