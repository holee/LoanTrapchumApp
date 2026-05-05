using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames;

public sealed class EntityNamesConfiguration : IBaseEntityTypeConfigurations<Domains.Models.Lists.EntityNames>
{
    public void Configure(EntityTypeBuilder<Domains.Models.Lists.EntityNames> builder)
    {
        builder.ToTable("lt_entityNames");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(250);
        builder.Property(x => x.FirstName).HasMaxLength(128);
        builder.Property(x => x.MiddleName).HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(128);
        builder.Property(x => x.AltName).HasMaxLength(300);
        builder.Property(x => x.Name).HasMaxLength(300);
        builder.Property(x => x.EntType).HasMaxLength(20);
        builder.Property(x => x.PassportId).HasMaxLength(20);
        builder.Property(x => x.Nationality).HasMaxLength(20);
        builder.Property(x => x.DateOfBirth);
        builder.Property(x => x.Salutation).HasMaxLength(15);
        builder.Property(x => x.Gender).HasMaxLength(15);
        builder.Property(x => x.JobTitle).HasMaxLength(50);
        builder.Property(x => x.Phone).HasMaxLength(200);
        builder.Property(x => x.AltPhone).HasMaxLength(20);
        builder.Property(x => x.EmailAddress).HasMaxLength(100);
        builder.Property(x => x.IdCard).HasMaxLength(30);
        builder.Property(x => x.CompanyName).HasMaxLength(100);
        builder.Property(x => x.MarritalStatus).HasMaxLength(20);
        builder.Property(x => x.CountryName).HasMaxLength(50);
        builder.Property(x => x.City).HasMaxLength(50);
        builder.Property(x => x.ProDescription).HasMaxLength(100);
        builder.Property(x => x.ProInKhmer).HasMaxLength(100);
        builder.Property(x => x.DisDescription).HasMaxLength(100);
        builder.Property(x => x.DisInKhmer).HasMaxLength(100);
        builder.Property(x => x.ComDescription).HasMaxLength(100);
        builder.Property(x => x.ComInKhmer).HasMaxLength(100);
        builder.Property(x => x.VillDescription).HasMaxLength(100);
        builder.Property(x => x.VilInKhmer).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(600);
        builder.Property(e => e.CreatedDt).HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedDt).HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.Customer)
            .WithOne(x => x.EntityNames)
            .HasForeignKey<Customer>(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Employee)
            .WithOne(x => x.EntityNames)
            .HasForeignKey<Employee>(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Vendor)
            .WithOne()
            .HasForeignKey<Vendor>(x => x.VendorId)
            .OnDelete(DeleteBehavior.Cascade);

    }

}