using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals;

public class CollateralConfiguration : IEntityTypeConfiguration<Collateral>
{
    public void Configure(EntityTypeBuilder<Collateral> builder)
    {
        builder.ToTable("lt_collaterals");
        builder.HasKey(x=>x.CollateralId);
        builder.Property(x=>x.DocumentKeeping).HasMaxLength(200);
        builder.Property(x=>x.RegisterDate).HasColumnType("DATET");
        builder.Property(x=>x.CustomerName).HasMaxLength(100);
        builder.HasMany(x => x.CollateralLines)
                .WithOne(x=>x.Collateral)
                .HasForeignKey(x => x.CollateralRefId);   
       
  
    }
}
