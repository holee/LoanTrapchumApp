using Domains.Models.Lists.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Vendors;

public sealed class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("lt_entVendors");
        builder.HasKey(x => x.VendorId);
        builder.Property(x => x.VendorId).HasMaxLength(250);
        builder.Property(x => x.CompanyName).HasMaxLength(200);
        builder.Property(x => x.CompanyAddress)
            .HasColumnName("CompanyAddress")
            .HasMaxLength(300);
    }

}