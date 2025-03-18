using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals
{
    public class CollateralSubTypeConfig : IEntityTypeConfiguration<CollateralSubType> 
    {
        public void Configure(EntityTypeBuilder<CollateralSubType> builder)
        {
            builder.ToTable("st_collateralSubTypes");
            builder.Property(x => x.CollateralSubTypeName).HasMaxLength(200);
            builder.Property(x => x.ValueRate).HasPrecision(18, 2);
            builder.Property(x => x.LoanSizeAmuntMax).HasPrecision(18, 2);
            builder.Property(x => x.LoanSizeAmuntMin).HasPrecision(18, 2);
        }
    }
}
