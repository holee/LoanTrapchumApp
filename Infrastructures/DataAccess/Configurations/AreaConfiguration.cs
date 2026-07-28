using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations;

public sealed class AreaConfiguration : IBaseEntityTypeConfigurations<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.ToTable("lt_areas");
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.AreaCode).HasMaxLength(50);
    }
}