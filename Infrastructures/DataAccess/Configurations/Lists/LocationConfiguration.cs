using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Lists;

public class LocationConfiguration : IEntityTypeConfiguration<LtLocation>
{
    public void Configure(EntityTypeBuilder<LtLocation> builder)
    {
        builder.ToTable("lt_locations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.LocationName)
                    .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                    .HasMaxLength(250);
        builder.Property(x => x.Latitude).HasMaxLength(250);
        builder.Property(x => x.Longitude).HasMaxLength(250);
        builder.Property(x => x.CreatedDT).HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.ModifiedDT).HasDefaultValueSql("GETDATE()");
    }
}
