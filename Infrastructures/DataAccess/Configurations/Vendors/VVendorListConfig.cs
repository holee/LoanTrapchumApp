using Applications.Responses.ViewModels.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Vendors
{
    public class VVendorListConfig : IEntityTypeConfiguration<VendorVm>
    {
        public void Configure(EntityTypeBuilder<VendorVm> builder)
        {
            builder.ToView("VVendors");
            builder.HasNoKey();
        }
    }
}
