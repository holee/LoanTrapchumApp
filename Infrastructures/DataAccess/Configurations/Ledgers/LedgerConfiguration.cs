using Domains.Models.Journals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Ledgers;

public class LedgerConfiguration : IEntityTypeConfiguration<Ledger>
{
    public void Configure(EntityTypeBuilder<Ledger> builder)
    {
        builder.ToTable("lt_Ledgers");
        builder.HasKey(l => l.LedgerId).HasName("LedgerId");
        builder.Property(l => l.LedgerId).ValueGeneratedOnAdd();
        builder.Property(l => l.LedgerCode).HasMaxLength(20);
        builder.Property(l => l.LedgerName).HasMaxLength(100);
        builder.Property(l => l.Description).HasMaxLength(200);
        builder.Property(l => l.IsMainGl);
        builder.Property(l => l.ParentRefId);
        builder.Property(l => l.CurrencyRefId);
        builder.Property(l => l.BankNumber).HasMaxLength(30);
        builder.Property(l => l.LedgerTypeRefId);
        builder.Property(l => l.IsActive).HasDefaultValue(true);
        builder.Property(l => l.IsConsolidate).HasDefaultValue(false);

        builder.HasOne(x => x.Currency)
                .WithMany(x=>x.Ledgers)
                 .HasForeignKey(fk => fk.CurrencyRefId);

        builder.HasOne(x => x.LedgerType)
            .WithMany(x => x.Ledgers)
            .HasForeignKey(fk => fk.LedgerTypeRefId);

        builder.HasOne(x => x.ParentLedger)
                        .WithMany()
                        .HasForeignKey(x => x.ParentRefId);
        builder.Navigation(x => x.Currency).AutoInclude();
        builder.Navigation(x => x.LedgerType).AutoInclude();
    }
}
