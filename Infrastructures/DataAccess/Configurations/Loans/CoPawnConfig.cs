using Domains.Models.Lists;
using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans;

public class CoPawnConfig : IEntityTypeConfiguration<CoPawnCustomer>
{
    public void Configure(EntityTypeBuilder<CoPawnCustomer> builder)
    {
        builder.ToTable("lt_CoPawnCustomers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BranchId).IsRequired(false);
        builder.Property(x => x.RequestId).HasMaxLength(250).IsRequired(false);
        builder.Property(x => x.CustomerId).HasMaxLength(250).IsRequired(false);
        builder.Property(x => x.LoanId).HasMaxLength(250).IsRequired(false);
        builder.Property(x => x.Status).HasDefaultValue(true);
        builder.Property(x => x.Description).HasMaxLength(400).IsRequired(false);
        builder.Property(x => x.CreatedDT).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.ModifiedDT).HasDefaultValueSql("GETUTCDATE()");
    }
}
