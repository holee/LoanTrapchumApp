using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class LoanRepaymentLineConfig : IEntityTypeConfiguration<LoanPaymentLine>
    {
        public void Configure(EntityTypeBuilder<LoanPaymentLine> builder)
        {
            builder.ToTable("tt_loanPaymentLines");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.LoanRefId).HasMaxLength(250);
            builder.Property(p => p.LoanScheduleRefId);
            builder.Property(p => p.InstallmentNo);
            builder.Property(p => p.PayDate).HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.DayArrea);
            builder.Property(p => p.PaidPrinciple).HasPrecision(18, 2);
            builder.Property(p => p.PaidInterest).HasPrecision(18, 2);
            builder.Property(p => p.PaidCollectionFee).HasPrecision(18, 2);
            builder.Property(p => p.PenaltyAmount).HasPrecision(18, 2);
            builder.Property(p => p.SubTotal).HasPrecision(18, 2);
            builder.Property(p => p.OfficialReceiptNo).HasMaxLength(20);
            builder.Property(p => p.Created).HasDefaultValueSql("GETDATE()");
        }
    }
}
