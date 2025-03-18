using Domains.Models.Transaction;
using Infrastructures.DataAccess.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.CashManagements
{
    public class TTCashVaultConfig : IEntityTypeConfiguration<CashVault>
    {
        public void Configure(EntityTypeBuilder<CashVault> builder)
        {
            builder.ToTable("tt_CashVaults");
            builder.HasKey(x => x.CashVaultsId);
            builder.Property(x => x.CashVaultsId).ValueGeneratedOnAdd();
            builder.Property(x => x.BranchRefId).HasColumnType("INT");
            builder.Property(x => x.BankRefId).HasColumnType("INT");
            builder.Property(x => x.TranRefId).HasColumnType("BIGINT");
            builder.Property(x => x.UserRefId).HasColumnType("BIGINT");
            builder.Property(x => x.CvStatusRefId).HasColumnType("INT").HasMaxLength(2);
            builder.Property(x => x.CvDate).HasColumnType("DATETIME");
            builder.Property(x => x.TransactionRefId)
                .HasConversion(ConversionExts.GuidConversion);
        }
    }
}
