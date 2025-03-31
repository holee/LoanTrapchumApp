using Applications.Extensions;
using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals;

public class CollateralLineConfig : IEntityTypeConfiguration<CollateralLine>
{
    public void Configure(EntityTypeBuilder<CollateralLine> builder)
    {
        builder.ToTable("lt_collateralLines");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.IsReturn).HasDefaultValue(false);
        builder.Property(x => x.IsAvailable).HasDefaultValue(true);
        builder.Property(x => x.MarketPrice).HasPrecision(18,3);
        builder.Property(x => x.HotPrice).HasPrecision(18, 3);
        builder.Property(x => x.PawnPrice).HasPrecision(18, 3);
        builder.Property(x => x.OwnerName).HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.WareHouse).HasMaxLength(200);
        
        builder.Property(p=>p.CollateralRefId)
            .HasConversion(TypeConversions.GuidConversion);
    }
}