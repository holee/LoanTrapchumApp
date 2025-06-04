using Domains.Models.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Addresses;

public class AddrProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.ToTable("lt_addrProvinces");
        builder.HasKey(x => x.ProvinceId);
        builder.Property(x => x.ProvinceId).ValueGeneratedOnAdd();
        builder.Property(x => x.CountryId);
        builder.Property(x => x.ProvinceCode).HasMaxLength(20);
        builder.Property(x => x.ProInKhmer).HasMaxLength(100);
        builder.Property(x => x.ProDescription).HasMaxLength(100);
    }
}
