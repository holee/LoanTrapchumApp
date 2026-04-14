using Applications.Features.Loans.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans;

//public sealed class VLoanCollateralsConfigurations : IBaseEntityTypeConfigurations<VLoanCollateral>
//{
//    public void Configure(EntityTypeBuilder<VLoanCollateral> builder)
//    {
//        builder.ToView("VLoanCollaterals");
//        builder.HasNoKey();
//        builder.Property(e => e.LoanId);
//        builder.Property(e => e.CollateralRefId);
//        builder.Property(e => e.TypeName).HasMaxLength(100);
//        builder.Property(e => e.SubTypeName).HasMaxLength(100);
//        builder.Property(e => e.MarketPrice).HasPrecision(18, 4);
//        builder.Property(e => e.PawnPrice).HasPrecision(18, 4);
//        builder.Property(e => e.HotPrice).HasPrecision(18, 4);

//    }
//}
