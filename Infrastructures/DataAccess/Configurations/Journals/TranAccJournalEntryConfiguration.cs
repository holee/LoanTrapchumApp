using Domains.Models.Journals;
using Infrastructures.DataAccess.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Journals;

public class TranAccJournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.ToTable("tt_accJournalEntries");
        builder.HasKey(x => x.JournalEntryId);
        builder.Property(j => j.JournalDate);
        builder.Property(j => j.ReferrenceNum);
        builder.Property(j => j.JournalDesc);
        builder.Property(j => j.CurrencyRefId);
        builder.Property(j => j.TotalDr).HasPrecision(18, 3);
        builder.Property(j => j.TotalCr).HasPrecision(18, 3);
        builder.Property(j => j.TotalBaseDr).HasPrecision(18, 3);
        builder.Property(j => j.TotalBaseCr).HasPrecision(18, 3);
        builder.Property(j => j.JournalLineCount).HasDefaultValue(1);
        builder.Property(j => j.CreatedDt).HasDefaultValueSql("GETDATE()");
        builder.Property(j => j.ModifiedDt).HasDefaultValueSql("GETDATE()");
        builder.Property(j => j.EditSeq).HasDefaultValue(0);
        builder.Property(j => j.IsAdjustment).HasDefaultValue(false);
        builder.Property(j => j.TransactionsRefId).HasConversion(ConversionExts.GuidConversion);


        builder.HasMany(x => x.JournalEntryLines)
                    .WithOne().HasForeignKey(fk => fk.JournalRefId);


        builder.HasOne(x => x.LoanTransaction)
                .WithOne(x => x.JournalEntry)
                .HasForeignKey<JournalEntry>(x => x.TransactionsRefId);
    }
}
