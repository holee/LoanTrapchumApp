using Applications.Features.Customers.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers;


public sealed class VCustomerDetailConfig : IBaseEntityTypeConfigurations<VCustomerDetails>
{
    public void Configure(EntityTypeBuilder<VCustomerDetails> builder)
    {
        builder.ToView("VCustomerList");
        builder.HasNoKey();
        builder.Property(x => x.CustomerId).HasMaxLength(250);
        builder.Property(x => x.FirstName).HasMaxLength(50);
        builder.Property(x => x.MiddleName).HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(50);
        builder.Property(x => x.FullName).HasMaxLength(150);
        builder.Property(x => x.AltName).HasMaxLength(150);
        builder.Property(x => x.EntType).HasMaxLength(20);
        builder.Property(x => x.PassportId).HasMaxLength(50);
        builder.Property(x => x.Nationality).HasMaxLength(50);
        builder.Property(x => x.Salutation).HasMaxLength(10);
        builder.Property(x => x.JobTitle).HasMaxLength(100);
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.AltPhone).HasMaxLength(20);
        builder.Property(x => x.EmailAddress).HasMaxLength(100);
        builder.Property(x => x.IdCard).HasMaxLength(50);
        builder.Property(x => x.CompanyName).HasMaxLength(200);
        builder.Property(x => x.MarritalStatus).HasMaxLength(20);
        builder.Property(x => x.SpouseName).HasMaxLength(150);
        builder.Property(x => x.SpouseNationality).HasMaxLength(50);
        builder.Property(x => x.SpousePassport).HasMaxLength(50);
        builder.Property(x => x.SpousePhoneNo).HasMaxLength(20);
        builder.Property(x => x.SpouseOccupation).HasMaxLength(100);
        builder.Property(x => x.Occupation).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.CountryName).HasMaxLength(50);
        builder.Property(x => x.City).HasMaxLength(50);
        builder.Property(x => x.ProInKhmer).HasMaxLength(100);
        builder.Property(x => x.DisInKhmer).HasMaxLength(100);
        builder.Property(x => x.ComInKhmer).HasMaxLength(100);
        builder.Property(x => x.VilInKhmer).HasMaxLength(100);
        builder.Property(x => x.Photo).HasMaxLength(500);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.IsLoanActive).HasDefaultValue(false);
        builder.HasQueryFilter(x => x.IsActive == true);

    }
}
