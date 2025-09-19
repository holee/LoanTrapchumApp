using Applications.Features.Currencies.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Currencies
{
    public class CurrencíesListConfiguration : IEntityTypeConfiguration<VCurrenciesList>
    {
        public void Configure(EntityTypeBuilder<VCurrenciesList> builder)
        {
            builder.ToView("VCurrenciesLists");
            builder.HasNoKey();
            builder.Property(p => p.CurrencyName).HasMaxLength(100);
            builder.Property(p => p.CurrencyCode).HasMaxLength(10);
            builder.Property(p => p.BuyingRate).HasPrecision(18, 3);
            builder.Property(p => p.SellingRate).HasPrecision(18, 3);
            builder.Property(p => p.SpotRate).HasPrecision(18, 3);
            builder.Property(p => p.IsBaseCurrency);
            builder.Property(p => p.IsActive).HasColumnType("BIT");
            builder.Property(p => p.AsOfDate).HasColumnType("DATE");
            builder.HasQueryFilter(p => p.IsActive==true);
        }
    }
}
