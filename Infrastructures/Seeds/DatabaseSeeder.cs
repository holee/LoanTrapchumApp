using Applications.Constants.Authentications;
using Applications.Constants.Authorizations;
using Applications.Extensions.Identity;
using Applications.Extensions.Identity.Enumerations;
using Applications.Helpers;
using Domains.Models.Identity;
using Infrastructures.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Seeds
{
    public sealed class DatabaseSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserDataContext _context;

        public DatabaseSeeder(UserDataContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task DatabaseSeederAsync()
        {
            await CheckDatabaseMigrationAsync();
            await SeedDefaultRolesWithPermissions();
            await SeedSystemUser();
            await SeedAdminUser();
            await SeedPermission();
        }

        private async Task CheckDatabaseMigrationAsync()
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                await _context.Database.MigrateAsync();
            }
        }

        private async Task SeedDefaultRolesWithPermissions()
        {
            var defaultRoles = Enum.GetValues<AppRole>().Select(x => x.ToString());

            var dict = Enum.GetValues(typeof(AppRole)).Cast<AppRole>()
                .ToDictionary(t => t.ToString(), t => t.GetDisplayName());

            foreach (var roleName in defaultRoles)
            {
                if (await _roleManager.FindByNameAsync(roleName) is not ApplicationRole role)
                {
                    var appRole = new ApplicationRole
                    {
                        Name = roleName,
                        IsActive = true,
                        Description = dict[roleName].ToString(),
                    };
                    await _roleManager.CreateAsync(appRole);
                }
            }

            var superAdminRole = await _roleManager.FindByNameAsync(SystemRole.SuperAdmin);

            if (superAdminRole is not null)
            {
                await AssignPermissionsToRoleAsync(superAdminRole, ApplicationPermissions.AdminRolePermissions);

                await AssignPermissionsToRoleAsync(superAdminRole, ApplicationPermissions.DashboardPermission);
            }


            var roles = _roleManager.Roles.Select(x => x.Name).ToArray();

            if (roles.Contains(AppRoles.Co))
            {
                var approle = await _roleManager.FindByNameAsync(AppRoles.Co);

                if (approle is not null)
                {
                    await AssignPermissionsToRoleAsync(approle, ApplicationPermissions.CoDashboard);
                }
            }

            if (roles.Contains(AppRoles.Account))
            {
                var approle = await _roleManager.FindByNameAsync(AppRoles.Account);

                if (approle is not null)
                {
                    await AssignPermissionsToRoleAsync(approle, ApplicationPermissions.AccountDashboard);
                }
            }

            if (roles.Contains(AppRoles.SeniorAccount))
            {
                var approle = await _roleManager.FindByNameAsync(AppRoles.SeniorAccount);

                if (approle is not null)
                {
                    await AssignPermissionsToRoleAsync(approle, ApplicationPermissions.AccountDashboard);
                }
            }

            if (roles.Contains(AppRoles.ChiefAccount))
            {
                var approle = await _roleManager.FindByNameAsync(AppRoles.ChiefAccount);

                if (approle is not null)
                {
                    await AssignPermissionsToRoleAsync(approle, ApplicationPermissions.AccountDashboard);
                }
            }

            if (roles.Contains(AppRoles.Teller))
            {
                var approle = await _roleManager.FindByNameAsync(AppRoles.Teller);

                if (approle is not null)
                {
                    await AssignPermissionsToRoleAsync(approle, ApplicationPermissions.AccountDashboard);
                }
            }
        }

        private async Task SeedSystemUser()
        {
            var adminUser = new ApplicationUser
            {
                IsActive = true,
                UserName = SystemUser.UserName,
                Email = SystemUser.Email,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                BranchRefId = 0,
                NameRefId = string.Empty,
            };
            if (!await _userManager.Users.AnyAsync(u => u.Email == adminUser.Email))
            {
                var password = new PasswordHasher<ApplicationUser>();

                adminUser.PasswordHash = password.HashPassword(adminUser, SystemUser.Password);

                await _userManager.CreateAsync(adminUser);

                if (!await _userManager.IsInRoleAsync(adminUser, SystemRole.SuperAdmin))
                {
                    await _userManager.AddToRoleAsync(adminUser, SystemRole.SuperAdmin);
                }
            }
        }

        private async Task SeedAdminUser()
        {
            var adminUser = new ApplicationUser
            {
                IsActive = true,
                UserName = SystemUser.AdminUserName,
                Email = SystemUser.AdminEmail,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                BranchRefId = 0,
                NameRefId = string.Empty,
            };
            if (!await _userManager.Users.AnyAsync(u => u.Email == adminUser.Email))
            {
                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, SystemUser.Password);
                await _userManager.CreateAsync(adminUser);
                if (!await _userManager.IsInRoleAsync(adminUser, SystemRole.Admin))
                {
                    await _userManager.AddToRoleAsync(adminUser, SystemRole.Admin);
                }
            }
        }


        private async Task SeedPermission()
        {
            var permissions = ApplicationPermissions.AllPermissions;

            var exsitingPersmissions = await _context.Permissions.ToListAsync();

            List<Permissions> ps = new();
            
            foreach (var per in permissions)
            {
                if (exsitingPersmissions.Any(x =>
                        x.Module == per.Module && 
                        x.Action == per.Action && 
                        x.Group == per.Group))
                {
                    continue;
                }
                
                ps.Add(new Permissions
                {
                    Description = per.Description,
                    Group = per.Group,
                    IsBasic = per.IsBasic,
                    Action = per.Action,
                    Module = per.Module,
                });
                
            }
            _context.Permissions.AddRange(ps);
            await _context.SaveChangesAsync();
        }

        private async Task AssignPermissionsToRoleAsync(ApplicationRole role, IReadOnlyList<AppPermission> permissions)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var permission in permissions)
            {
                if (!currentClaims.Any(claim => claim.Type == AppClaim.Permission && claim.Value == permission.For))
                {
                    await _context.RoleClaims.AddAsync(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        Module = permission.Module,
                        ClaimType = AppClaim.Permission,
                        ClaimValue = permission.For,
                        Description = permission.Description,
                        Group = permission.Group
                    });

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}


//if (roleName.Contains(SystemRole.SuperAdmin))
//{
//    await AssignPermissionsToRoleAsync(appRole, RoleLevelPermissions.AllPermissions);
//}

//var permissions = ClaimStore.SystemClaims; //Enum.GetValues<Permission>().Select(x=> new Claim(x.ToString(),"1"));

//var roleClaims = await _roleManager.GetClaimsAsync(superAdminRole);

//if (roleClaims.Any())
//{
//    foreach (var item in roleClaims)
//    {
//        await _roleManager.RemoveClaimAsync(superAdminRole, item);
//    }
//}

//foreach (var item in permissions)
//{
//    await _roleManager.AddClaimAsync(superAdminRole, new Claim(item.Type, "1"));
//}