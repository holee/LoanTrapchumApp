using Domains.Models.Lists.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Vendors;

public class VCustomerNoLoanConfig : IEntityTypeConfiguration<CustomerNoLoan>
{
    public void Configure(EntityTypeBuilder<CustomerNoLoan> builder)
    {
        builder.ToView("VCustomersNoLoan");
        builder.HasNoKey();
        builder.Property(x => x.CustomerId);
        builder.Property(x => x.FullName).HasMaxLength(100);
        builder.Property(x => x.AltName).HasMaxLength(100);
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.Address).HasMaxLength(200);
        builder.Property(x => x.Salutation).HasMaxLength(10);
        builder.Property(x => x.IsLoanActive);
        builder.Property(x => x.IdCard).HasMaxLength(50);
        builder.Property(x => x.DateOfBirth).HasColumnType("DATE");
        builder.Property(x => x.Phone).HasMaxLength(100);
        builder.Property(x => x.Circle).HasDefaultValue(0);
        builder.HasQueryFilter(x => x.IsLoanActive==false);
    }
}