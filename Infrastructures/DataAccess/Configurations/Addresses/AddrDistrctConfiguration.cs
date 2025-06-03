using Domains.Models.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Addresses;

public class AddrDistrctConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.ToTable("lt_addrDistricts");
        builder.HasKey(x => x.DistrictId);
        builder.Property(p => p.DistrictId).ValueGeneratedNever();
        builder.Property(p => p.DisDescription).HasMaxLength(100);
        builder.Property(p => p.DisInKhmer).HasMaxLength(100);
        builder.Property(p => p.ProvinceRefId);

    }
}
