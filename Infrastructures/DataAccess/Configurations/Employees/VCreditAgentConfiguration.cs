using Applications.Responses.ViewModels.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Employees
{
    public class VCreditAgentConfiguration : IEntityTypeConfiguration<VCreditAgent>
    {
        public void Configure(EntityTypeBuilder<VCreditAgent> builder)
        {
            builder.ToView("VCreditAgents");
            builder.HasNoKey();
            builder.HasQueryFilter(x=>x.IsRemove==false);
        }
    }
}
