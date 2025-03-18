using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals
{
    public class CollateralValuetionsConfig : IEntityTypeConfiguration<CollateralEvaluations>
    {
        public void Configure(EntityTypeBuilder<CollateralEvaluations> builder)
        {
            builder.ToTable("lt_CollateralEvaluations");
            builder.Property(x => x.ValueBy).HasMaxLength(200);
            builder.Property(x => x.ValueAmount).HasPrecision(18, 2);
            builder.Property(x => x.ValueDate).HasColumnType("DATE");
        }
    }
}
