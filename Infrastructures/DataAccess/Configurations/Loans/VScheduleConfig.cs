using Applications.Responses.ViewModels.Loans.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class VScheduleConfig : IEntityTypeConfiguration<VSchedule>
    {
        public void Configure(EntityTypeBuilder<VSchedule> builder)
        {
            builder.ToView("VSchedules");
            builder.HasNoKey();
            builder.Property(x => x.Principle).HasPrecision(18, 3);
            builder.Property(x => x.Interest).HasPrecision(18, 3);
            builder.Property(x => x.LoanOutstanding).HasPrecision(18, 3);
            builder.Property(x => x.CollectionFee).HasPrecision(18, 3);
            builder.Property(x => x.TotalPay).HasPrecision(18, 3);
            builder.Property(x => x.LeftPrinciple).HasPrecision(18, 3);
            builder.Property(x => x.PaidAmount).HasPrecision(18, 3);
            builder.Property(x => x.PaidCollectionFee).HasPrecision(18, 3);
            builder.Property(x => x.PaidInterest).HasPrecision(18, 3);
            builder.Property(x => x.PaidPrinciple).HasPrecision(18, 3);
            builder.Property(x => x.LeftInterestAmount).HasPrecision(18, 3);
            builder.Property(x => x.LeftCollectionFeeAmount).HasPrecision(18, 3);
            builder.Property(x => x.PenaltyRate).HasPrecision(18, 3);
            builder.Property(x => x.PenaltyAmount).HasPrecision(18, 3);
            builder.Property(x => x.LeftAmount).HasPrecision(18, 3);
            builder.Property(x => x.PaymentDate).HasColumnType("DATE");
        }
    }
}
