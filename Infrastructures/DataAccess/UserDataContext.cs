using Domains.Models.Identity;
using Infrastructures.Constants.Schemas;
using Infrastructures.DataAccess.Configurations.Auths;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructures.DataAccess
{
    public class UserDataContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public UserDataContext(DbContextOptions<UserDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users", SchemaNames.Security);
            builder.Entity<UserProfiles>().ToTable("UserProfiles", SchemaNames.Security);
            builder.Entity<ApplicationRole>().ToTable("Roles", SchemaNames.Security);
            builder.Entity<ApplicationRoleClaim>().ToTable("RoleClaims", SchemaNames.Security);
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", SchemaNames.Security);
            builder.Entity<ApplicationUserClaim>().ToTable("UserClaims", SchemaNames.Security);
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", SchemaNames.Security);
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", SchemaNames.Security);
            builder.ApplyConfiguration(new PermissionsConfigurations());



            builder.Entity<UserProfiles>()
                .HasKey(x => new { x.UserRefId });

            builder.Entity<ApplicationUser>()
                .HasOne<UserProfiles>()
                .WithOne()
                .HasForeignKey<UserProfiles>(u => u.UserRefId);

            //builder.Entity<ApplicationUser>()
            //    .HasOne<UserLoanSize>()
            //    .WithOne()
            //    .HasForeignKey<UserLoanSize>(uc => uc.UserRefId)
            //    .IsRequired(false);
        }

        public DbSet<ApplicationRoleClaim> ApplicationRoleClaim { get; set; }
        public DbSet<ApplicationUserClaim> ApplicationUserClaim { get; set; }
        public DbSet<UserProfiles> UserProfiles { get; set; }
        public DbSet<Permissions> Permissions => Set<Permissions>();
    }
}