using Domains.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings;

public sealed class AppSettingConfiguratios : IBaseEntityTypeConfigurations<AppSettings>
{
    public void Configure(EntityTypeBuilder<AppSettings> builder)
    {
        builder.ToTable("lt_settings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Code).HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(400);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
    }
}