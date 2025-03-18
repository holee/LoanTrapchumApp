using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings
{
    public class StOccupationConfiguration : IEntityTypeConfiguration<Occupation>
    {
        public void Configure(EntityTypeBuilder<Occupation> builder)
        {
            builder.ToTable("st_occupation");
            builder.HasKey(dep => dep.OccupationId);
            builder.Property(p => p.OccupationTitle).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.Khmer).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.TypeOf).HasMaxLength(50).IsRequired(false);

            builder.HasData(new Occupation[]
            {

            });

        }
    }
}
