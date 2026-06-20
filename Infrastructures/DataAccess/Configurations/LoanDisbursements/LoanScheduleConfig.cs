using Domains.Models.Loans;
using Infrastructures.DataAccess.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{

    public class LoanScheduleConfig : IEntityTypeConfiguration<LoanSchedule>
    {
        public void Configure(EntityTypeBuilder<LoanSchedule> builder)
        {

            builder.ToTable("tt_loanSchedules");
            builder.HasKey(k => k.LoanScheduleId);
            builder.Property(x => x.LoanScheduleId).ValueGeneratedOnAdd();
            builder.Property(x => x.TotalFee)
                    .HasComputedColumnSql("isnull([Interest],(0))+isnull([CollectionFee],(0))", stored: true);
            builder.Property(x => x.ScheduleStatusRefId)
                                  .HasDefaultValue(2);
            builder.Property(x => x.PaymentDate)
                                    .HasColumnType("DATE");
            builder.Property(x => x.Principal)
                                    .HasPrecision(18, 2);
            builder.Property(x => x.Interest)
                                    .HasPrecision(18, 2);
            builder.Property(x => x.TotalPay)
                                    .HasPrecision(18, 2);
            builder.Property(x => x.PaidPrinciple)
                                    .HasPrecision(18, 2);
            builder.Property(x => x.PaidInterest)
                                    .HasPrecision(18, 2);
            builder.Property(x => x.TotalPaid)
                                    .HasPrecision(18, 2);
            builder.Property(x => x.OtherCharge)
                                    .HasPrecision(18, 2)
                                    .HasDefaultValue(0.00M);
            builder.Property(x => x.LoanOutstanding)
                                    .HasPrecision(18, 2);
            builder.Property(x => x.TmpPaidPrincipal)
                                  .HasPrecision(18, 2)
                                  .HasDefaultValue(0.00M);
            builder.Property(x => x.TmpPaidInterest)
                                    .HasPrecision(18, 2)
                                    .HasDefaultValue(0.00M);
            builder.Property(x => x.InterestRate)
                                    .HasPrecision(18, 2);
            builder.Property(x => x.CollectionFee)
                                    .HasPrecision(18, 2)
                                    .HasDefaultValue(0.00M);
            builder.Property(x => x.PaidRiskFee)
                                    .HasPrecision(18, 2)
                                    .HasDefaultValue(0.00M);
            builder.Property(x => x.PaidCollectionFee)
                                    .HasPrecision(18, 2)
                                    .HasDefaultValue(0.00M);
            builder.Property(x => x.PaidRiskFee)
                                    .HasPrecision(18, 2)
                                    .HasDefaultValue(0.00M);
            builder.Property(x => x.TmpPaidCollectionFee)
                                    .HasPrecision(18, 2)
                                    .HasDefaultValue(0.00M);
            builder.Property(x => x.TmpPaidRiskFee)
                                    .HasPrecision(18, 2)
                                    .HasDefaultValue(0.00M);
            builder.Property(x => x.RiskFee)
                                   .HasPrecision(18, 2)
                                   .HasDefaultValue(0.00M);
            builder.Property(x => x.BatchName).HasMaxLength(400);

            builder.Property(x => x.LoanRefId).HasMaxLength(250);

            builder.Property(x => x.AgentRefId).HasMaxLength(250);


            builder.Property(x=>x.PaymentDate).HasColumnType("DATETIME")
                                                 .HasConversion(
                                                        v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                                                        v => v.HasValue ? new DateTimeOffset(v.Value, TimeSpan.Zero) : (DateTimeOffset?)null
                                                    );

            //    .HasConversion(ConversionExts.GuidConversion);

        }
    }
}
