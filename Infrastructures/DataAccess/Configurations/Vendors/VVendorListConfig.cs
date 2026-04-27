using Applications.Features.Vendors.Dto.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Vendors
{
    public class VVendorListConfig : IEntityTypeConfiguration<VendorDto>
    {
        public void Configure(EntityTypeBuilder<VendorDto> builder)
        {
            builder.ToView("VVendors");
            builder.HasNoKey();
        }
    }
}
