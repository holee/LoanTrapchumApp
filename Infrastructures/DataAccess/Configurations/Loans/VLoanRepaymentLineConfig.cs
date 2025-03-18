using Applications.Responses.ViewModels.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans;

public class VLoanRepaymentLineConfig : IEntityTypeConfiguration<VLoanRepaymentLine>
{
    public void Configure(EntityTypeBuilder<VLoanRepaymentLine> builder)
    {
        builder.ToView("VLoanRepaymentLines");
        builder.HasNoKey();
        builder.Property(x => x.PaidPrinciple).HasPrecision(18, 2);
        builder.Property(x => x.PaidInterest).HasPrecision(18, 2);
        builder.Property(x => x.PaidCollectionFee).HasPrecision(18, 2);
        builder.Property(x => x.PenaltyAmount).HasPrecision(18, 2);
        builder.Property(x => x.SubTotal).HasPrecision(18, 2);
    }
}
