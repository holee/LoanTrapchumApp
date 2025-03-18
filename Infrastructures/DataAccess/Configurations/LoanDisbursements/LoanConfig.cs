using Domains.Models.Loans;
using Infrastructures.DataAccess.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public class LoanConfig : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("tt_loanDisbursements");
            builder.HasKey(x => x.LoanDisbursementsId);
            builder.Property(x => x.LoanDisbursementsId).HasDefaultValueSql("NEWID()");
            builder.Property(x => x.IsGroupLoan).HasDefaultValue(false);
            builder.Property(x => x.Principle).HasPrecision(18, 2);
            builder.Property(x => x.Remark).HasMaxLength(400);
            builder.Property(x => x.InstallmentType).HasMaxLength(20);
            builder.Property(x => x.LoanAccountCode).HasMaxLength(50);
            builder.Property(x => x.AdminFeeRate).HasPrecision(18, 2);
            builder.Property(x => x.InterestRate).HasPrecision(18, 2);
            builder.Property(x => x.AccruedArreaRate).HasPrecision(18, 2);
            builder.Property(x => x.ArrearRate).HasPrecision(18, 2);
            builder.Property(x => x.CollectionFeeRate).HasPrecision(18, 2);
            builder.Property(x => x.PaidInterest).HasPrecision(18, 2);
            builder.Property(x => x.AccruedAmount).HasPrecision(18, 2);
            builder.Property(x => x.Adjustment).HasPrecision(18, 2);
            builder.Property(x => x.AccruedBalance).HasPrecision(18, 2);
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

            builder.Property(x => x.TransactionRefId)
                   .HasConversion(ConversionExts.GuidConversion);

            builder.Property(x => x.LoanRequestRefId)
                     .HasConversion(ConversionExts.GuidConversion);
           
            builder.HasMany(x => x.LoanSchedules)
                    .WithOne()
                    .HasForeignKey(x => x.LoanDisbursementRefId);

            builder.HasOne(x => x.LoanRequest)
                   .WithOne()
                   .HasForeignKey<Loan>(x => x.LoanRequestRefId);


            builder.HasOne(x => x.LoanTransaction)
                .WithOne()
                .HasForeignKey<Loan>(x => x.TransactionRefId);

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