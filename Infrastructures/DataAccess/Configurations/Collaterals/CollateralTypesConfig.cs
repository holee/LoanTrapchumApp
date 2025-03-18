using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals
{
    public class CollateralTypesConfig : IEntityTypeConfiguration<CollateralType>
    {
        public void Configure(EntityTypeBuilder<CollateralType> builder)
        {
            builder.ToTable("st_collateralTypes");
            builder.HasKey(x => x.CollateralTypeId);
            builder.Property(x => x.CollateralTypeId).ValueGeneratedNever();
            builder.Property(x => x.CollateralTypeCode).HasMaxLength(100);
            builder.Property(x => x.IsActive).HasDefaultValue(false);
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.HasQueryFilter(x => x.IsActive == true);
        }
    }
}
