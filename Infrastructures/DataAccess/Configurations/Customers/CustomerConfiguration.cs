using Domains.Models;
using Domains.Models.Collaterals;
using Domains.Models.Lists.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {

        builder.ToTable("lt_entcustomers");
        builder.HasKey(x => x.CustomerId);
        builder.Property(p => p.CustomerInfId).ValueGeneratedOnAdd();
        builder.Property(p => p.CustomerId).HasMaxLength(250);
        builder.Property(p => p.Dependents).HasMaxLength(100);
        builder.Property(p => p.NoOfChildren).HasDefaultValue(0);
        builder.Property(p => p.Income).HasPrecision(18, 3);
        builder.Property(p => p.Expenditure).HasPrecision(18, 3);
        builder.Property(p => p.SpouseName).HasMaxLength(50);
        builder.Property(p => p.SpouseDateOfBirth).HasMaxLength(20);
        builder.Property(p => p.SpouseNationality).HasMaxLength(50);
        builder.Property(p => p.SpousePassport).HasMaxLength(20);
        builder.Property(p => p.SpousePhoneNo).HasMaxLength(20);
        builder.Property(p => p.SpouseOccupation).HasMaxLength(100);

        builder.HasOne(x => x.Collateral)
                 .WithOne(x => x.Customer)
                    .HasForeignKey<Collateral>(x => x.CollateralId);

        builder.HasOne(x => x.EmergencyContact)
                 .WithOne()
                    .HasForeignKey<EmergencyContact>(x => x.ContactId)
                    .IsRequired(false);

        builder.HasMany(x => x.CustomerExpenses)
               .WithOne(x => x.Customer)
                  .HasForeignKey(x => x.CustomerRefId)
                  .IsRequired(false);

        builder.HasMany(x => x.CustomerIncomes)
               .WithOne(x => x.Customer)
                  .HasForeignKey(x => x.CustomerRefId)
                  .IsRequired(false);

        builder.HasMany(x => x.LoanRequests)
            .WithOne(x => x.Customer)
            .HasForeignKey(fk => fk.CustomerRefId);



        builder.HasMany(x => x.LoanDisbursments)
            .WithOne()
            .HasForeignKey(fk => fk.CustomerRefId);



        builder.Navigation(x => x.EmergencyContact).AutoInclude();
    }
}