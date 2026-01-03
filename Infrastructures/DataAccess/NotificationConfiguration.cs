using Domains.Models;
using Infrastructures.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess
{
    public sealed class NotificationConfiguration : IBaseEntityTypeConfigurations<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Message).HasMaxLength(250);
            builder.Property(p => p.Title).HasMaxLength(250);
            builder.Property(p => p.Url).HasMaxLength(1000);
            builder.Property(p => p.Type)
                        .HasConversion<string>()
                        .HasMaxLength(250);
            builder.Property(p => p.ReferenceId).HasMaxLength(250);
            builder.Property(p => p.UserId).HasMaxLength(450);
            builder.Property(p => p.IsRead).HasDefaultValue(false);
            builder.Property(x => x.CreatedAt)
              .HasDefaultValueSql("GETUTCDATE()")
              .ValueGeneratedOnAdd();
        }
    }
}
