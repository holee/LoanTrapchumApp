using Domains.Models.Currencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Currencies
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("lt_currencies");
            builder.HasKey(pk => pk.CurrencyId);
            builder.Property(p => p.CurrencyId).ValueGeneratedOnAdd();
            builder.Property(p => p.CurrencyCode);
            builder.HasIndex(p => p.CurrencyCode).IsUnique();
            builder.Property(p => p.CurrencyType);
            builder.Property(p => p.ExchangeRate).HasPrecision(18, 2);
            builder.Property(p => p.SpotRate).HasPrecision(18, 2);
            builder.Property(p => p.Limited).HasPrecision(18, 2);
            builder.Property(p => p.IsBaseCurrency).HasPrecision(18, 2);
            builder.Property(p => p.Description);

            builder.HasMany(s => s.SpotRates)
                        .WithOne(c => c.Currency)
                            .HasForeignKey(fk => fk.CurrencyRefId);

            builder.HasData(new Currency[]
            {
                new Currency
                {
                    CurrencyId=1,
                    CurrencyCode="KHR",
                    CurrencyType="L",
                    ExchangeRate=4000.00M,
                    IsBaseCurrency=false,
                    Description="Khmer Riel",
                    Limited=20.00M,
                    SpotRate=1.00M
                },
                new Currency
                {
                    CurrencyId=2,
                    CurrencyCode="USD",
                    CurrencyType="B",
                    ExchangeRate=1.00M,
                    IsBaseCurrency=true,
                    Description="US Dollar",
                    Limited=20.00M,
                    SpotRate=1.00M
                }

            });




        }
    }
}
