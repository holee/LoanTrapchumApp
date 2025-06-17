using Domains.Models.Journals;
using Domains.Models.Loans;
using Domains.Models.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Transactions
{
    public class TranLoanTransactionConfiguration : IBaseEntityTypeConfigurations<LoanTransaction>
    {
        public void Configure(EntityTypeBuilder<LoanTransaction> builder)
        {
            builder.ToTable("tt_transactions");
            builder.HasKey(x => x.TransactionsId);
            builder.Property(p => p.TranTypeRefId);
            builder.Property(p => p.TransactionTypeName);
            builder.Property(p => p.TransactionDate).HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.UserRefId);
            builder.Property(p => p.CreatedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.ModifiedDt).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.JournalEntry)
                        .WithOne()
                        .HasForeignKey<JournalEntry>(fk => fk.TransactionsRefId);

            builder.HasMany(c => c.CashManagements)
                       .WithOne()
                       .HasForeignKey(fk => fk.TransactionRefId);

            builder.HasMany(x => x.ForeignExchanges)
                       .WithOne()
                       .HasForeignKey(fk => fk.TransactionRefId);

            builder.HasOne(x => x.OpenBranch)
                      .WithOne()
                      .HasForeignKey<OpenBranch>(fk => fk.TransactionRefId);


            builder.HasOne(x => x.CashVault)
                       .WithOne()
                       .HasForeignKey<CashVault>(fk => fk.TransactionRefId);



            builder.HasOne(x => x.LoanPayment)
                .WithOne(x => x.LoanTransaction)
                .HasForeignKey<LoanPayment>(fk => fk.TransactionRefId)
                .IsRequired(false);
        }

    }
}
