using Domains.Models.Journals;
using Infrastructures.DataAccess.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Journals
{
    public class TranAccJournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
    {
        public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
        {
            builder.ToTable("tt_accJournalEntryLines");
            builder.HasKey(p => p.JournalLineId);
            builder.Property(p => p.JournalLineId).ValueGeneratedOnAdd();
            builder.Property(p => p.JournalRefId);
            builder.Property(p => p.LineSeq);
            builder.Property(p => p.EntityNameRefId);
            builder.Property(p => p.LedgerRefId);
            builder.Property(p => p.Debit).HasPrecision(18, 2);
            builder.Property(p => p.Credit).HasPrecision(18, 2);
            builder.Property(p => p.Amount).HasPrecision(18, 2)
                   .HasComputedColumnSql("((ISNULL([Debit],0)) - (ISNULL([Credit],0)))", true);
            builder.Property(p => p.ExchangeRate).HasColumnType("DECIMAL(18,2)");
            builder.Property(p => p.BaseAmount).HasColumnType("DECIMAL(18,2)");
            builder.Property(p => p.JournalLineDesc);
        }
    }
}
