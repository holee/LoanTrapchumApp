using Applications.Responses.ViewModels.Currencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.CashManagements
{
    public class VCashVaultBalanceConfig : IEntityTypeConfiguration<VCashVaultBalance>
    {
        public void Configure(EntityTypeBuilder<VCashVaultBalance> builder)
        {
            builder.HasNoKey();
            builder.Property(p => p.CurrencyName).HasMaxLength(200);
            builder.Property(p => p.CurrencyCode).HasMaxLength(20);
            builder.Property(p => p.VaultBalance).HasPrecision(18, 2);
        }
    }
}
