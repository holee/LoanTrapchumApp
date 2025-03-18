using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class CoPawnConfig : IEntityTypeConfiguration<CoPawnCustomer>
    {
        public void Configure(EntityTypeBuilder<CoPawnCustomer> builder)
        {
            builder.ToTable("lt_CoPawnCustomers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CustomerRefId).IsRequired(false);
            builder.Property(x => x.LoanRefId).IsRequired(false);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
        }
    }
}
