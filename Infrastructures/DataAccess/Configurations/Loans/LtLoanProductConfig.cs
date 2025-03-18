using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class LtLoanProductConfig : IEntityTypeConfiguration<LoanProduct>
    {
        public void Configure(EntityTypeBuilder<LoanProduct> builder)
        {
            builder.ToTable("lt_loanProduct");
            builder.HasKey(x => x.LoanProductId);
            builder.Property(prop => prop.LoanProductId).ValueGeneratedOnAdd();
            builder.Property(prop => prop.LoanProductName).HasMaxLength(250);
            builder.Property(prop => prop.LoanProductCode).HasMaxLength(50);
            builder.Property(prop => prop.LoanSize).HasMaxLength(20);
            builder.Property(prop => prop.CurrencyRefId);
            builder.Property(prop => prop.Installments).HasMaxLength(15);
            builder.Property(prop => prop.InstallmentType).HasMaxLength(50);
            builder.Property(prop => prop.InterestRate).HasMaxLength(15);
            builder.Property(prop => prop.RepaymentMode).HasMaxLength(15);
            builder.Property(prop => prop.OriginationFees).HasMaxLength(20);
            builder.Property(prop => prop.ArrearRate).HasMaxLength(15);
            builder.Property(prop => prop.LoanBflfolId);
            builder.Property(prop => prop.LoanAptId);
            builder.Property(prop => prop.CreatedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(prop => prop.ModifiedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(prop => prop.EditSeq).HasDefaultValue(0);
            builder.Property(prop => prop.IsActive).HasDefaultValue(true);


            builder.HasOne(x => x.Currency)
                       .WithMany(x => x.LoanProducts)
                           .HasForeignKey(x => x.CurrencyRefId);
            //builder.Navigation(nameof(Currency)).AutoInclude();
        }
    }
}
