using Domains.Models.Transaction;
using Infrastructures.DataAccess.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Transactions
{
    public class TranOpenBranchConfig : IEntityTypeConfiguration<OpenBranch>
    {
        public void Configure(EntityTypeBuilder<OpenBranch> builder)
        {
            builder.ToTable("tt_OpenBranches");
            builder.HasKey(x => x.OpenBranchId);
            builder.Property(x => x.OpenBranchId).ValueGeneratedOnAdd();
            builder.Property(x => x.OpenStatus).HasColumnType("CHAR").HasMaxLength(1);
            builder.Property(x => x.IsContinue).HasColumnType("CHAR")
                                                .HasMaxLength(1)
                                                .HasDefaultValueSql("N");
            builder.Property(x => x.OpenDate).HasColumnType("DATETIME");
            builder.Property(x => x.TransactionRefId)
                .HasConversion(ConversionExts.GuidConversion);
        }
    }
}
