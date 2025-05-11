using Domains.Models.Lists.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Vendors
{
    public class LTVendorConfig : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ToTable("lt_vendors");
            builder.HasKey(x => x.VendorId);
            builder.Property(x => x.VendorId).HasMaxLength(250);
            builder.Property(c => c.CompanyName).HasMaxLength(200);
        }
    }
}
