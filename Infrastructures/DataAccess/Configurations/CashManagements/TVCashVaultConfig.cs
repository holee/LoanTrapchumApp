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
            builder.Property(x => x.BranchRefId).HasColumnType("INT");
            builder.Property(x => x.BankRefId).HasColumnType("INT");
            builder.Property(x => x.StartCash).HasPrecision(18, 2);
            builder.Property(x => x.VaultBalance).HasPrecision(18,2);
            builder.Property(x => x.VaultIn).HasPrecision(18, 2);
            builder.Property(x => x.VaultOut).HasPrecision(18, 2);
        }
    }
}
