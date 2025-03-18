using Domains.Models.Currencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Currencies
{
    public class LtSpotRateConfig : IEntityTypeConfiguration<CurrencySpotRate>
    {
        public void Configure(EntityTypeBuilder<CurrencySpotRate> builder)
        {
            builder.ToTable("lt_CurrencySpotRates");
            builder.HasKey(x => x.SpotRateId);
            builder.Property(x => x.SpotRateId).ValueGeneratedOnAdd();
            builder.Property(x => x.AvgRate).HasPrecision(18, 2);
            builder.Property(x => x.BuyingRate).HasPrecision(18, 2);
            builder.Property(x => x.SellingRate).HasPrecision(18, 2);
            builder.Property(x => x.TransactionDate).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.SpotRate).HasPrecision(18, 2);
            builder.Property(x => x.Description).HasMaxLength(200);

        }
    }
}
