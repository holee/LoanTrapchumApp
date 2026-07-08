using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public sealed class LoanConfigurations : IEntityTypeConfiguration<LoanDisbursment>
    {
        private const string SQL= "COALESCE([Principle],0)-COALESCE([AdminFeeAmount],0)";
        public void Configure(EntityTypeBuilder<LoanDisbursment> builder)
        {
            builder.ToTable("tt_loanDisbursements");
            builder.HasKey(x => x.LoanId);
            builder.Property(x => x.LoanId).HasMaxLength(250);
            builder.Property(x => x.LoanAccountCode).HasMaxLength(50);
            builder.Property(x => x.IsGroupLoan).HasDefaultValue(false);
            builder.Property(x => x.Principle).HasPrecision(18, 2);
            builder.Property(x => x.Remark).HasMaxLength(400);
            builder.Property(x => x.InstallmentType).HasMaxLength(20);
            builder.Property(x => x.AdminFeeRate).HasPrecision(18, 2);
            builder.Property(x => x.InterestRate).HasPrecision(18, 2);
            builder.Property(x => x.AccruedArreaRate).HasPrecision(18, 2);
            builder.Property(x => x.ArrearRate).HasPrecision(18, 2);
            builder.Property(x => x.CollectionFeeRate).HasPrecision(18, 2);
            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2)
                   .HasComputedColumnSql(SQL, stored:true);
            builder.Property(x => x.PaidInterest).HasPrecision(18, 2);
            builder.Property(x => x.AccruedAmount).HasPrecision(18, 2);
            builder.Property(x => x.Adjustment).HasPrecision(18, 2);
            builder.Property(x => x.AccruedBalance).HasPrecision(18, 2);
            builder.Property(x => x.AccIdInsurance).HasColumnName("AccID_Insurance");
            builder.Property(x => x.InsurranceNew).HasPrecision(18, 2);
            builder.Property(x => x.LastAccrued).HasPrecision(18, 2);
            builder.Property(x => x.TruePrinciple).HasPrecision(18, 2);
            builder.Property(x => x.TmpAmount).HasPrecision(18, 2);
            builder.Property(x => x.Premium).HasPrecision(18, 2);
            builder.Property(x => x.ProvisionForPrinciple).HasPrecision(18, 2);
            builder.Property(x => x.ProvisionForAccr).HasPrecision(18, 2);
            builder.Property(x => x.RiskFeeRate).HasPrecision(18, 2);
            builder.Property(x => x.SavingMeada).HasPrecision(18, 2);
            builder.Property(x => x.SavingCompousalry).HasPrecision(18, 2);
            builder.Property(x => x.LastBalance).HasPrecision(18, 2);
            builder.Property(x => x.PaidPrinciple).HasPrecision(18, 2);
            builder.Property(x => x.InsInstallment).HasPrecision(18, 2);
            builder.Property(x => x.InsurranFee).HasPrecision(18, 2);
            builder.Property(x => x.AdminFeeAmount).HasPrecision(18, 2);
            builder.Property(x => x.SavingNew).HasPrecision(18, 2);
            builder.Property(x => x.StartDate).HasColumnType("DATE");
            builder.Property(x => x.LastPaymentDate).HasColumnType("DATE");
            builder.Property(x => x.TransactionsRefId).HasMaxLength(250);
            builder.Property(x => x.LoanRequestId).HasMaxLength(250);

            builder.Property(x => x.AllowEdit).HasDefaultValue(false);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.RequestEdit).HasDefaultValue(false);

            builder.HasMany(x => x.LoanSchedules)
                    .WithOne()
                    .HasForeignKey(x => x.LoanRefId);

            builder.HasOne(x => x.LoanRequest)
                   .WithOne()
                   .HasForeignKey<LoanDisbursment>(x => x.LoanRequestId);

            builder.HasOne(x => x.LoanTransaction)
                .WithOne()
                .HasForeignKey<LoanDisbursment>(x => x.TransactionsRefId);

        }
    }
}

//builder.HasMany(c => c.CollateralLines)
//        .WithMany(l => l.LoanDisbursements)
//        .UsingEntity<CollateralLink>(
//            j => j.HasOne(x => x.CollateralLine)
//                  .WithMany()
//                  .HasForeignKey(fk => fk.CollateralLineRefId),
//            j => j.HasOne(l => l.LoanDisbursement)
//                 .WithMany()
//                 .HasForeignKey(x => x.LoanRefId));


//builder.HasMany(g => g.Guarantors)
//        .WithMany(l => l.LoanDisbursements)
//        .UsingEntity<LoanGuarantor>(
//            j => j.HasOne(x => x.Guarantor)
//                    .WithMany()
//                    .HasForeignKey(fk => fk.EntityNameRefId),
//            j => j.HasOne(l => l.LoanDisbursement)
//                .WithMany()
//                .HasForeignKey(fk => fk.LoanRefId)
//        );

//builder.HasOne(x => x.Customer)
//            .WithMany(x => x.LoanDisbursements)
//                .HasForeignKey(x => x.CustomerRefId);

//builder.HasOne(x => x.LoanCreditAgent)
//           .WithMany(x => x.LoanDisbursements)
//               .HasForeignKey(x => x.CreditAgentRefId);