using Domains.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Apps
{
    public class SettingConfigurations : IEntityTypeConfiguration<AppSettings>
    {
        public void Configure(EntityTypeBuilder<AppSettings> builder)
        {
            builder.ToTable("lt_settings");
            builder.HasKey(x => x.Id);
            builder.Property(p=>p.IsActive).HasDefaultValue(1);
            builder.Property(p=>p.Code).HasMaxLength(10);
            builder.Property(p=>p.Description).HasMaxLength(200);
            builder.Property(p=>p.Value).HasMaxLength(200); 
            builder.HasQueryFilter(p => p.IsActive == true);
        }
    }
}
