using Domains.Models.Journals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Journals;

public sealed class OperationJournalEntryConfiguration : IBaseEntityTypeConfigurations<OperationsJournalEntry>
{
    public void Configure(EntityTypeBuilder<OperationsJournalEntry> builder)
    {
        builder.ToTable("tt_opJournalEntries", "dbo");
        builder.HasKey(e => e.OpJournalEntryId);
        builder.Property(e => e.OpJournalEntryId).HasMaxLength(250);
        builder.Property(e => e.TransactionsRefId).HasMaxLength(250);
        builder.Property(e => e.BranchRefId).HasColumnType("int");
        builder.Property(e => e.BankRefId).HasColumnType("int");
        builder.Property(e => e.JournalType).HasMaxLength(50);
        builder.Property(e => e.JournalDate).HasColumnType("date");
        builder.Property(e => e.IsAdjustment).HasColumnType("bit");
        builder.Property(e => e.ReferrenceNum).HasMaxLength(50);
        builder.Property(e => e.CurrencyRefId).HasColumnType("int");
        builder.Property(e => e.ExchangeRate).HasColumnType("decimal(18,3)").HasPrecision(18, 3);
        builder.Property(e => e.TotalDr).HasColumnType("decimal(18,3)").HasPrecision(18, 3);
        builder.Property(e => e.TotalCr).HasColumnType("decimal(18,3)").HasPrecision(18, 3);
        builder.Property(e => e.TotalBaseDr).HasColumnType("decimal(18,3)").HasPrecision(18, 3);
        builder.Property(e => e.TotalBaseCr).HasColumnType("decimal(18,3)")
            .HasPrecision(18, 3);
        builder.Property(e => e.JournalLineCount)
            .HasColumnType("int");
        builder.Property(e => e.EditSeq)
            .HasColumnType("int");
        builder.Property(e => e.JournalDesc)
            .HasColumnType("nvarchar(255)");
        builder.Property(e => e.CreatedDt)
            .HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedDt)
            .HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
    }
}