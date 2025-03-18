using Domains.Models.Currencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Currencies;

public class CurrencyCounterConfiguration : IEntityTypeConfiguration<CurrencyCounter>
{
    public void Configure(EntityTypeBuilder<CurrencyCounter> builder)
    {
        builder.ToTable("lt_currencyNotes");
        builder.HasKey(x => x.Id);
        builder.Property(c => c.CurrencyRefId);
        builder.Property(c => c.CurrencyValue).HasPrecision(18, 2);
        builder.Property(c => c.Description).HasMaxLength(200);
        builder.Property(c => c.Quantity);
        builder.HasOne(x => x.Currency)
                        .WithMany(c => c.CurrencyCounters)
                            .HasForeignKey(x => x.CurrencyRefId);
    }
}

