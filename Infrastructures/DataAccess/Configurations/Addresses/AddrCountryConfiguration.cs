using Domains.Models.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Addresses;

public class AddrCountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("lt_addrCountries");
        builder.HasKey(c => c.CountryId).HasName("CountryId");
        builder.Property(c => c.CountryId).ValueGeneratedNever();
        builder.Property(c => c.CountryName).HasMaxLength(100);
        builder.Property(c => c.CountryCode).HasMaxLength(100);
        builder.Property(c => c.DesInKhmer).HasMaxLength(100);
    }
}