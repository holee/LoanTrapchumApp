using Domains.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Auths
{
    public sealed class UserProfilesConfiguration : IEntityTypeConfiguration<UserProfiles>
    {
        public void Configure(EntityTypeBuilder<UserProfiles> builder)
        {
            builder.ToTable("UserProfiles");
            builder.HasKey(x => x.UserRefId);
            builder.Property(p => p.UserRefId).HasMaxLength(250);
            builder.Property(p => p.FileName).HasMaxLength(400);
            builder.Property(p => p.Created).HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.IsActive).HasDefaultValue<bool>(true);
        }
    }
}
