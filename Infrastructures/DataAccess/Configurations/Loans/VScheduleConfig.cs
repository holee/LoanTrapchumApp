using Applications.Responses.ViewModels.Loans.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class VScheduleConfig : IBaseEntityTypeConfigurations<VSchedule>
    {
        public void Configure(EntityTypeBuilder<VSchedule> builder)
        {
            builder.ToView("VSchedules");
            builder.HasKey(x => new { x.LoanScheduleId,x.LoanId,x.BranchId });
            builder.Property(x => x.Principle).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.Interest).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.InterestRate).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.LoanOutstanding).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.CollectionFee).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.TotalPay).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.LeftPrinciple).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.PaidAmount).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.PaidCollectionFee).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.PaidInterest).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.PaidPrinciple).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.LeftInterestAmount).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.LeftCollectionFeeAmount).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.PenaltyRate).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.PenaltyAmount).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.LeftAmount).HasColumnType("DECIMAL(18,3)").HasPrecision(18, 3);
            builder.Property(x => x.PaymentDate).HasColumnType("DATE");
        }
    }
}
