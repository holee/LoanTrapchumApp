using Domains.Models.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Addresses;


public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("lt_addresses");
        builder.HasKey(x => x.AddressId);
        builder.Property(a => a.AddressId).ValueGeneratedOnAdd();
        builder.Property(a => a.CountryName).HasMaxLength(100);
        builder.Property(a => a.City).HasMaxLength(100);
        builder.Property(a => a.ProvinceRefId);
        builder.Property(a => a.ProDescription).HasMaxLength(100);
        builder.Property(a => a.ProInKhmer).HasMaxLength(100);
        builder.Property(a => a.DistrictRefId);
        builder.Property(a => a.DisDescription).HasMaxLength(100);
        builder.Property(a => a.DisInKhmer).HasMaxLength(100);
        builder.Property(a => a.CommuneRefId);
        builder.Property(a => a.ComDescription).HasMaxLength(100);
        builder.Property(a => a.ComInKhmer).HasMaxLength(100);
        builder.Property(a => a.VillageRefId);
        builder.Property(a => a.VillDescription).HasMaxLength(100);
        builder.Property(a => a.VilInKhmer).HasMaxLength(100);

        //builder.HasOne(x => x.EntityName)
        //            .WithOne(emp => emp.Address)
        //                .HasForeignKey<EntityName>(fk => fk.AddressRefId);
    }
}