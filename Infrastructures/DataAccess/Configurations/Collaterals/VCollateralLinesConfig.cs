using Applications.Features.Collaterals.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals;

public class VCollateralLinesConfig : IBaseEntityTypeConfigurations<VCollateralLines>
{
    public void Configure(EntityTypeBuilder<VCollateralLines> builder)
    {
        builder.ToView("VCollateralLines");
        builder.HasNoKey();
        builder.Property(x => x.CollateralId).HasColumnName("CollateralId");
        builder.Property(x => x.IsReturn).HasDefaultValue(false);
        builder.Property(x => x.IsAvailable).HasDefaultValue(true);
        builder.Property(x => x.MarketPrice).HasPrecision(18, 3);
        builder.Property(x => x.HotPrice).HasPrecision(18, 3);
        builder.Property(x => x.PawnPrice).HasPrecision(18, 3);
        builder.Property(x => x.ValueRate).HasPrecision(18, 3);
        builder.Property(x => x.OwnerName).HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.WareHouse).HasMaxLength(200);
        builder.Property(x => x.RegisterDate).HasColumnName("RegisterDate");
        builder.Property(x => x.SubTypeName)
                    .HasColumnName("CollateralSubTypeName")
                    .HasMaxLength(250);//SubTypeName
        builder.Property(x => x.IsUsed).HasColumnName("IsUsed");
    }
}