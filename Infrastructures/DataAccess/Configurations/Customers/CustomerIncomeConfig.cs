using Domains.Models.Lists.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public class CustomerIncomeConfig : IEntityTypeConfiguration<CustomerIncome>
    {
        public void Configure(EntityTypeBuilder<CustomerIncome> builder)
        {
            builder.ToTable("lt_customerIncomes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.IncomeSource).HasMaxLength(100);
            builder.Property(x => x.Amount).HasPrecision(18, 2);
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(p => p.CustomerRefId).HasConversion(TypeConversions.GuidConversion);
            builder.HasOne(x => x.Customer)
                        .WithMany(x => x.CustomerIncomes)
                        .HasForeignKey(x => x.CustomerRefId);
          
        }
    }
}
