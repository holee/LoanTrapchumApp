using Applications.Responses.ViewModels.Loans.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public sealed class LoanPaymentConfiguration : IBaseEntityTypeConfigurations<LoanPaymentVm>
    {
        public void Configure(EntityTypeBuilder<LoanPaymentVm> builder)
        {
            builder.ToView("VLoanPayments");
            builder.HasNoKey();
            builder.Property(lp => lp.Id)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(lp => lp.TransactionRefId)
                .HasMaxLength(250);
            builder.Property(lp => lp.LoanRefId)
                .HasMaxLength(250);
            builder.Property(lp => lp.CreditAgentRefId)
                .HasMaxLength(250);
            builder.Property(lp => lp.UserRefId)
                .HasMaxLength(250);
            builder.Property(lp => lp.OfficialReceiptNo)
                .HasMaxLength(30);
            builder.Property(lp => lp.PayDate)
                .HasColumnType("datetime");
            builder.Property(lp => lp.Principle)
                .HasColumnType("decimal(18,3)");
            builder.Property(lp => lp.Interest)
                .HasColumnType("decimal(18,3)");
            builder.Property(lp => lp.CollectionFeeAmount)
                .HasColumnType("decimal(18,3)");
            builder.Property(lp => lp.PenaltyAmount)
                .HasColumnType("decimal(18,3)");
            builder.Property(lp => lp.SubTotal)
                .HasColumnType("decimal(18,3)");
            builder.Property(lp => lp.Created)
                .HasColumnType("datetime");
            builder.Property(lp => lp.IsReturn)
                .IsRequired(false);
        }
    }
}
