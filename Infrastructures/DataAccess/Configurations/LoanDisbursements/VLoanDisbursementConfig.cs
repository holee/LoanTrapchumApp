using Applications.Responses.ViewModels.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public class VLoanDisbursementConfig : IEntityTypeConfiguration<VLoanDisbursments>
    {
        public void Configure(EntityTypeBuilder<VLoanDisbursments> builder)
        {
            
            builder.ToView("VLoanDisbursments");
            builder.HasNoKey();
            builder.Property(x => x.CustomerId);
            builder.Property(x => x.CustomerName).HasMaxLength(250);
            builder.Property(x => x.Salutation).HasMaxLength(20);
            builder.Property(x => x.Phone).HasMaxLength(20);
            builder.Property(x => x.Amount).HasPrecision(18, 3);
            builder.Property(x => x.InterestRate).HasPrecision(18, 3);
            builder.Property(x => x.CollectionFeeRate).HasPrecision(18, 3);
            builder.Property(x => x.AdminFeeRate).HasPrecision(18, 3);
            builder.Property(x => x.Amount).HasPrecision(18, 3);
            builder.Property(x => x.PaidPrinciple).HasPrecision(18, 3);
            builder.Property(x => x.TotalCollectionFee).HasPrecision(18, 3);
            builder.Property(x => x.TotalInterest).HasPrecision(18, 3);
            builder.Property(x => x.AdminFeeAmount).HasPrecision(18, 3);
            builder.Property(x => x.TotalOtherCharge).HasPrecision(18, 3);
            builder.Property(x => x.OutStanding).HasPrecision(18, 3);
            builder.Property(x => x.CurrencyName).HasMaxLength(30);
            builder.Property(x => x.AgentName).HasMaxLength(100);
            builder.HasQueryFilter(x => x.LoanStatus == "Active");
        }
    }
}
