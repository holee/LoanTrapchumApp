using Applications.Extensions.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Auths
{
    public sealed class UserProfilesConfiguration : IEntityTypeConfiguration<UserProfiles>
    {
        public void Configure(EntityTypeBuilder<UserProfiles> builder)
        {
            builder.ToTable("UserProfiles");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.FileTitle).HasMaxLength(400);
            builder.Property(p => p.FileName).HasMaxLength(400);
            builder.Property(p => p.Created).HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.IsActive).HasDefaultValue<bool>(true);
            builder.HasOne(u => u.AppUser)
                .WithMany()
                .HasForeignKey(fk => fk.UserRefId);
        
        }
    }
}
