using Applications.Responses.ViewModels.Loans.Disbursements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames
{
    public class VGuarantorLoanConfiguration : IBaseEntityTypeConfigurations<VGuarantorLoan>
    {
        public void Configure(EntityTypeBuilder<VGuarantorLoan> builder)
        {
            builder.ToView("VGuarantorLoans");
            builder.HasKey(x => new { x.Id, x.GuarantorId });
            builder.Property(x => x.CustomerName).HasMaxLength(100);
            builder.Property(x => x.Principle).HasPrecision(18, 3);
            builder.Property(x => x.InterestRate).HasPrecision(18, 3);
            builder.Property(x => x.CollectionFeeRate).HasPrecision(18, 3);
            builder.Property(x => x.AdminFeeRate).HasPrecision(18, 3);
            builder.Property(x => x.PaidPrinciple).HasPrecision(18, 3);
            builder.Property(x => x.PaidInterest).HasPrecision(18, 3);
            builder.Property(x => x.OutStanding).HasPrecision(18, 3);
        }
    }




}
