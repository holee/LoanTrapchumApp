using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals;

public class VLoanRequestConfiguration : IEntityTypeConfiguration<VLoanRequests>
{ 
    public void Configure(EntityTypeBuilder<VLoanRequests> builder)
    {
        builder.ToView("VLoanRequests");
        builder.HasNoKey();
        builder.Property(x => x.AdminFeeAmount).HasPrecision(18, 3);
        builder.Property(x => x.RequestAmount).HasPrecision(18, 3);
        builder.Property(x => x.InterestRate).HasPrecision(18, 3);
        builder.Property(x => x.CollectionFeeRate).HasPrecision(18, 3);
        builder.Property(x => x.FirstPaymentDate).HasColumnType("DATE");
        builder.Property(x => x.LastPaymentDate).HasColumnType("DATE");
        builder.Property(x => x.RequestDate).HasColumnType("DATE");
    }
}
