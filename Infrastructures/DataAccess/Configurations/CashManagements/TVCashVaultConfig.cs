using Applications.Features.CashManagements.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.CashManagements
{
    public class TVCashVaultConfig : IBaseEntityTypeConfigurations<VCashVaults> 
    {
        public void Configure(EntityTypeBuilder<VCashVaults> builder)
        {
            builder.ToView("VCashVaults");
            builder.HasNoKey();
            builder.Property(x => x.CurrencyCode).HasMaxLength(250);
            builder.Property(x => x.BranchRefId);
            builder.Property(x => x.BankRefId);
            builder.Property(x => x.StartCash).HasColumnType("DECIMAL(18,3)");
            builder.Property(x => x.VaultBalance).HasColumnType("DECIMAL(18,3)");
            builder.Property(x => x.UserBalance).HasColumnType("DECIMAL(18,3)");
            builder.Property(x => x.VaultIn).HasColumnType("DECIMAL(18,3)");
            builder.Property(x => x.VaultOut).HasColumnType("DECIMAL(18,3)");
        }
    }
}
