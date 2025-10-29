using Applications.Features.Collaterals.Dtos;
using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals
{
    public class VCollateralConfig : IBaseEntityTypeConfigurations<VCollateral>
    {
        public void Configure(EntityTypeBuilder<VCollateral> builder)
        {
            builder.ToView("VCollateralList");
            builder.HasKey(x => x.CustomerId);
            builder.Property(x => x.CustomerName).HasMaxLength(100);
            builder.Property(x => x.RegisterDate).HasColumnType("DATE");
            builder.Property(x => x.Remark).HasMaxLength(400);
        }
    }


    public class VLoanCollateralLineConfig : IBaseEntityTypeConfigurations<VLoanRequestCollateralLines>
    {
        public void Configure(EntityTypeBuilder<VLoanRequestCollateralLines> builder)
        {
            builder.ToView("VLoanCollaterals");
            builder.HasNoKey();
            builder.Property(x => x.HotPrice).HasPrecision(18, 3);
            builder.Property(x => x.MarketPrice).HasPrecision(18, 3);
            builder.Property(x => x.PawnPrice).HasPrecision(18, 3);
            //builder.Property(x => x.).HasPrecision(18, 3);
            //builder.Property(x => x.HotPrice).HasPrecision(18, 3);
        }
    }
}
