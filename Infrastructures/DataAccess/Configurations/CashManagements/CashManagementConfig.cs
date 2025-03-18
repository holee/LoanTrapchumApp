using Domains.Models.Transaction;
using Infrastructures.DataAccess.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.CashManagements
{
    public class CashManagementConfig : IEntityTypeConfiguration<CashManagement>
    {
        public void Configure(EntityTypeBuilder<CashManagement> builder)
        {
            builder.ToTable("tt_CashManagements");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.BankRefId).HasColumnType("INT");
            builder.Property(x => x.StaffRefId).HasColumnType("BIGINT");
            builder.Property(x => x.CurrencyRefId).HasColumnType("INT");
            builder.Property(x => x.BranchRefId).HasColumnType("INT");
            builder.Property(x => x.TranRefId).HasColumnType("BIGINT");
            builder.Property(x => x.Amount).HasPrecision(8, 2);
            builder.Property(x => x.Balance).HasPrecision(8, 2);
            builder.Property(x => x.CashStatusRefId).HasColumnType("INT");
            builder.Property(x => x.TransactionDate).HasColumnType("DATETIME");
            builder.Property(x => x.BusinessDate).HasColumnType("DATETIME");
            builder.Property(x => x.Reject).HasColumnType("CHAR(1)");
            builder.Property(x => x.Receive).HasColumnType("CHAR(1)");
            builder.Property(x => x.Remark).HasMaxLength(250);
            builder.Property(x => x.TmpAmount).HasColumnType("DECIMAL(18,2)");
            builder.Property(x => x.DelegateToStaffRefId).HasColumnType("BIGINT");

            builder.Property(x => x.TransactionRefId)
                  .HasConversion(ConversionExts.GuidConversion);


            builder.HasMany(x => x.CashManagementDetails)
                .WithOne()
                .HasForeignKey(x => x.CashmanagmentRefId);

        }
    }
}
