using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals
{
    public class CollateralEvaluationsConfig : IEntityTypeConfiguration<CollateralEvaluations>
    {
        public void Configure(EntityTypeBuilder<CollateralEvaluations> builder)
        {
            builder.ToTable("lt_CollateralEvaluations");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.CollateralRefId).HasMaxLength(250);
        }
    }
}
