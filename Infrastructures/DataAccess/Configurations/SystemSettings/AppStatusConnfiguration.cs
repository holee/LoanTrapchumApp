using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings
{
    public class AppStatusConnfiguration : IEntityTypeConfiguration<AppStatus>
    {
        public void Configure(EntityTypeBuilder<AppStatus> builder)
        {
            builder.ToTable("Statuses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).HasMaxLength(30).IsRequired(false);
            builder.Property(x => x.Description).HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.StatusOf).HasMaxLength(50).IsRequired(false);
        }
    }
}
