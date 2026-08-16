using Applications.Features.Ledgers.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Ledgers;

public class VLedgerConfiguration : IEntityTypeConfiguration<VLedger>
{
    public void Configure(EntityTypeBuilder<VLedger> builder)
    {
        builder.ToView("VLedgers");
        builder.HasKey(x => x.LedgerId);
        builder.Property(x => x.LedgerId).ValueGeneratedOnAdd();
        builder.Property(x => x.ParentLedgerName).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.LedgerName).HasMaxLength(100);
        builder.Property(x => x.LedgerTypeCode).HasMaxLength(20);
        builder.HasQueryFilter(x => x.IsActive == true);
    }
}
