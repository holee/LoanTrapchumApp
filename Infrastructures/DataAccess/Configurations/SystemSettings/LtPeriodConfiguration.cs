using Domains.Models;
using Infrastructures.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings;

public sealed class LtPeriodConfiguration : IBaseEntityTypeConfigurations<LtPeriod>
{
    public void Configure(EntityTypeBuilder<LtPeriod> builder)
    {
        builder.ToTable("lt_periods");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Period).IsRequired();
    }
}
