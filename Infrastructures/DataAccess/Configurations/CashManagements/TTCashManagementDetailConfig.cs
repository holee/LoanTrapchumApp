using Domains.Models.Transaction;
using Infrastructures.DataAccess.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.CashManagements
{
    public class TTCashManagementDetailConfig : IEntityTypeConfiguration<CashManagementDetail>
    {
        public void Configure(EntityTypeBuilder<CashManagementDetail> builder)
        {
            builder.ToTable("tt_CashManagementDetails");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CachmanagementRefId)
                   .HasMaxLength(450);
            builder.Property(x => x.BranchRefId).HasColumnType("INT");
            builder.Property(x => x.BankRefId).HasColumnType("INT");
            builder.Property(x => x.TransactionsRefId).HasMaxLength(250);
            builder.Property(x => x.CurrencyRefId).HasColumnType("INT");
            builder.Property(x => x.CurrencyRefName).HasMaxLength(50);
            builder.Property(x => x.CurrencyNoteRefId).HasMaxLength(50);
            builder.Property(x => x.CurrencyNoteValue).HasColumnType("INT");
            builder.Property(x => x.Quantity).HasColumnType("INT");
            builder.Property(x => x.TotalValue).HasColumnType("INT").HasComputedColumnSql("[Quantity] * [CurrencyNoteValue]", stored: true);
        }
    }
}
