using Applications.Responses.Lists.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals;

public class VCollateralLineConfig : IBaseEntityTypeConfigurations<VCollateralLines>
{
    public void Configure(EntityTypeBuilder<VCollateralLines> builder)
    {
        builder.ToTable("VCustomerCollateralLines");
        builder.HasKey(x => new { x.Id, x.CollateralId });
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.IsReturn).HasDefaultValue(false);
        builder.Property(x => x.IsAvailable).HasDefaultValue(true);
        builder.Property(x => x.MarketPrice).HasPrecision(18, 3);
        builder.Property(x => x.HotPrice).HasPrecision(18, 3);
        builder.Property(x => x.PawnPrice).HasPrecision(18, 3);
        builder.Property(x => x.OwnerName).HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.WareHouse).HasMaxLength(200);
        builder.Property(x => x.CollateralSubTypeName).HasMaxLength(250);
        builder.Property(x => x.IsUsed).HasColumnName("IsInUsed");
    }
}