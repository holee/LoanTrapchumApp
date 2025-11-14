using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domains.Models.Lists.Customers;

namespace Infrastructures.DataAccess.Configurations.Customers;

public class IcomeExpenseConfiguration : IBaseEntityTypeConfigurations<IncomeExpense>
{
    public void Configure(EntityTypeBuilder<IncomeExpense> builder)
    {
        builder.ToTable("lt_IcomeExpenses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
        builder.Property(x => x.Description)
                .HasColumnName("Description")
                .IsUnicode()
                .HasMaxLength(400);
        builder.Property(x => x.Types)
                .HasColumnName("Types")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsUnicode();
    }
}