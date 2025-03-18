using Domains.Models.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public class LoanTransactionTypeConfiguration : IEntityTypeConfiguration<LoanTransactionType>
    {
        public void Configure(EntityTypeBuilder<LoanTransactionType> builder)
        {
            builder.ToTable("st_transactionTypes");
            builder.Property(p => p.TransactionTypeId).HasColumnName("TransactionTypeId");
            builder.Property(p => p.TransactionCode).HasColumnName("TransactionCode")
                            .HasMaxLength(50)
                                   .IsRequired(false);
            builder.Property(p => p.Description).HasColumnName("Description")
                                            .HasMaxLength(200)
                                                .IsRequired(false);
            builder.Property(p => p.IsCommission).HasColumnName("IsCommission")
                                                    .HasDefaultValue(false);




        }
    }
}
