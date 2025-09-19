using Domains.Models.Transaction;
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
            builder.Property(x => x.CashVaultsId).HasMaxLength(250);
            builder.Property(x => x.BranchRefId).HasColumnType("INT");
            builder.Property(x => x.BankRefId).HasColumnType("INT");
            builder.Property(x => x.CvStatusRefId).HasColumnType("INT").HasMaxLength(2);
            builder.Property(x => x.CvDate).HasColumnType("DATETIME");
            builder.Property(x => x.TransactionRefId).HasMaxLength(250);
            builder.Property(x => x.UserRefId).HasMaxLength(250);
        }
    }
}
