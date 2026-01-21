using Applications.Features.Loans.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public sealed class VLoanDisbursementCollateralConfig : IBaseEntityTypeConfigurations<VLoanDisbursementCollateral>
    {
        public void Configure(EntityTypeBuilder<VLoanDisbursementCollateral> builder)
        {
            builder.HasNoKey();
            builder.ToView("VLoanDisbursementCollaterals");
            builder.Property(x => x.MarketPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.HotPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.PawnPrice).HasColumnType("decimal(18,2)");
        }
    }
}
