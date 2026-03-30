using Applications.Features.Schedules.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans;

public class LoanPaymentLineVmConfiguration : IEntityTypeConfiguration<PaymentLineDetailsDto> 
{
    public void Configure(EntityTypeBuilder<PaymentLineDetailsDto> builder)
    {
        builder.ToView("VLoanPaymentLines");

        builder.HasNoKey();

        builder.Property(x => x.LoanPaymentRefId)
            .HasMaxLength(250);

        builder.Property(x => x.LoanRefId)
            .HasMaxLength(250);

        builder.Property(x => x.LoanScheduleRefId);

        builder.Property(x => x.BranchRefId);

        builder.Property(x => x.InstallmentNo);

        builder.Property(x => x.NumberOfDay);

        builder.Property(x => x.PayDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.DayArrea);

        builder.Property(x => x.OfficialReceiptNo)
            .HasMaxLength(20);
        builder.Property(x => x.PaidPrinciple)
            .HasColumnType("decimal(18,3)");

        builder.Property(x => x.PaidInterest)
            .HasColumnType("decimal(18,3)");

        builder.Property(x => x.PaidCollectionFee)
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.PenaltyAmount)
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.SubTotal)
            .HasColumnType("decimal(18,3)");

        builder.Property(x => x.Created)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");
    }
}
