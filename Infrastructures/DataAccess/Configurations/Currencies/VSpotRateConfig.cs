using Applications.Responses.ViewModels.Currencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Currencies
{
    public class VSpotRateConfig : IEntityTypeConfiguration<VSpotRate>
    {
        public void Configure(EntityTypeBuilder<VSpotRate> builder)
        {
            builder.ToView("VSpotRates");
            builder.HasNoKey();
            builder.Property(p => p.CurrencyName).HasMaxLength(100);
            builder.Property(p => p.SpotRate).HasColumnType("DECIMAL(18,2)");
            builder.Property(p => p.AvgRate).HasColumnType("DECIMAL(18,2)");
            builder.Property(p => p.BuyingRate).HasColumnType("DECIMAL(18,2)");
            builder.Property(p => p.SellingRate).HasColumnType("DECIMAL(18,2)");
            builder.Property(p => p.TransactionDate).HasColumnType("DATE");
            builder.Property(p => p.Used).HasColumnType("BIT");
        }
    }
}
