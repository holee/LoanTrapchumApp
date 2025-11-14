using Applications.Responses.ViewModels.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Addresses
{
    public class VDistrictConfig : IEntityTypeConfiguration<VDistricts>
    {
        public void Configure(EntityTypeBuilder<VDistricts> builder)
        {
            builder.ToView("VAddrDistricts");
            builder.HasNoKey();
        }
    }
    

    
}
