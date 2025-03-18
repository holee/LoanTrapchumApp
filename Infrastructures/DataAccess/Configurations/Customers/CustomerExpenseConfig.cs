using Domains.Models.Lists.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public class CustomerExpenseConfig : IEntityTypeConfiguration<CustomerExpense>
    {
        public void Configure(EntityTypeBuilder<CustomerExpense> builder)
        {
            builder.ToTable("lt_customerExpenses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.ExpenseTitle).HasMaxLength(100);
            builder.Property(x => x.Amount).HasPrecision(18, 2);
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.HasOne(x => x.Customer)
                    .WithMany(x => x.CustomerExpenses)
                        .HasForeignKey(x => x.CustomerRefId);
        }
    }
}
