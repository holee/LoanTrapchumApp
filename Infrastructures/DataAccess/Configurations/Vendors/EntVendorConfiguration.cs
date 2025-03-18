using Domains.Models.Lists.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Vendors
{
    public class EntVendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ToTable("lt_entVendors");
            builder.HasKey(x => x.VendorId);
            builder.Property(c => c.CompanyName).HasMaxLength(200);
        }
    }
}
