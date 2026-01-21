using Domains.Models.Currencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Currencies
{
    public class CurrencySpotRateConfig : IEntityTypeConfiguration<CurrencySpotRate>
    {
        public void Configure(EntityTypeBuilder<CurrencySpotRate> builder)
        {
            builder.ToTable("lt_CurrencySpotRates");
            builder.HasKey(k => k.SpotRateId);
            builder.Property(p => p.SpotRateId).ValueGeneratedOnAdd();
            builder.Property(p => p.TransactionDate).HasColumnType("DATE").HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.CreatedDt).HasColumnType("DATETIMEOFFSET(7)").HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.ModifiedDt).HasColumnType("DATETIMEOFFSET(7)").HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.SpotRate).HasPrecision(18, 5);
            builder.Property(p => p.BuyingRate).HasPrecision(18, 5);
            builder.Property(p => p.SellingRate).HasPrecision(18, 5);
            builder.Property(p => p.Description).HasMaxLength(200);


            builder.HasOne(x => x.Currency)
                        .WithMany(x => x.SpotRates)
                            .HasForeignKey(x => x.CurrencyRefId);
        }
    }
}
