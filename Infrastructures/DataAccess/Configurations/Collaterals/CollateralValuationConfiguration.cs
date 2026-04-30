using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals
{
    public class CollateralValuationConfiguration : IEntityTypeConfiguration<CollateralEvaluations>
    {
        public void Configure(EntityTypeBuilder<CollateralEvaluations> builder)
        {
            builder.ToTable("lt_CollateralEvaluations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CollateralRefId).HasMaxLength(250);
            builder.Property(x => x.ValueBy).HasMaxLength(250);
            builder.Property(x => x.ValueAmount).HasPrecision(18, 2);
            builder.Property(x => x.ValueDate).HasColumnType("DATE");
            builder.Property(x => x.CreatedBy).HasMaxLength(250);
            builder.Property(x => x.CreatedDT).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.ModifiedDT).HasDefaultValueSql("GETDATE()");
        }
    }
}
