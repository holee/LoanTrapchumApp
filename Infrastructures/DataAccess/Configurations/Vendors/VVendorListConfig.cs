using Applications.Responses.ViewModels.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Vendors
{
    public class VVendorListConfig : IEntityTypeConfiguration<VendorVm>
    {
        public void Configure(EntityTypeBuilder<VendorVm> builder)
        {
            builder.ToView("vVendorLists");
            builder.Property(c => c.CountryName).HasMaxLength(200).HasColumnName("Vendor_CountryName");
            builder.Property(c => c.City).HasColumnName("Vendor_City");
            //builder.Property(c => c.ProvinceRefId).HasColumnName("Vendor_ProvinceRefId");
            //builder.Property(c => c.ProDescription).HasColumnName("Vendor_ProDescription");
            builder.Property(c => c.ProInKhmer).HasColumnName("Vendor_ProInKhmer");
            //builder.Property(c => c.DistrictRefId).HasColumnName("Vendor_DistrictRefId");
            //builder.Property(c => c.DisDescription).HasColumnName("Vendor_DisDescription");
            builder.Property(c => c.DisInKhmer).HasColumnName("Vendor_DisInKhmer");
            //builder.Property(c => c.CommuneRefId).HasColumnName("Vendor_CommuneRefId");
            //builder.Property(c => c.ComDescription).HasColumnName("Vendor_ComDescription");
            builder.Property(c => c.ComInKhmer).HasColumnName("Vendor_ComInKhmer");
            //builder.Property(c => c.VillageRefId).HasColumnName("Vendor_VillageRefId");
            //builder.Property(c => c.VillDescription).HasColumnName("Vendor_VillDescription");
            builder.Property(c => c.VilInKhmer).HasColumnName("Vendor_VilInKhmer");
            builder.HasNoKey();
        }
    }
}
