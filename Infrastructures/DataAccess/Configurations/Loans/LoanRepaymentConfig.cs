using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class LoanRepaymentConfig : IEntityTypeConfiguration<LoanRepayment>
    {
        public void Configure(EntityTypeBuilder<LoanRepayment> builder)
        {
            builder.ToTable("tt_LoanRepayments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.LoanRefId);
            builder.Property(p => p.PayDate);
            builder.Property(p => p.Principle).HasPrecision(18, 2);
            builder.Property(p => p.Interest).HasPrecision(18, 2);
            builder.Property(p => p.CollectionFeeAmount).HasPrecision(18, 2);
            builder.Property(p => p.PenaltyAmount).HasPrecision(18, 2);
            builder.Property(p => p.SubTotal).HasPrecision(18, 2);
            builder.Property(p => p.OfficialReceiptNo).HasMaxLength(30);
            builder.HasMany(fk => fk.LoanRepaymentLines)
                    .WithOne()
                    .HasForeignKey(x => x.RepaymentRefId);
        }
    }
}
