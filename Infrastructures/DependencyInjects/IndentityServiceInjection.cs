using Applications.Extensions.Identity;
using Applications.Interfaces.Addresses;
using Applications.Interfaces.Branches;
using Applications.Interfaces.Employees;
using Applications.Interfaces.Identity;
using CoreBussiness.Implements;
using Infrastructures.Data;
using Infrastructures.Implements;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructures.DependencyInjections
{
    public static class IndentityServiceInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<UserDataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            //services.AddHttpContextAccessor();
            services.AddScoped<Seeds.DatabaseSeeder>();
            services.AddAuthorization();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<UserDataContext>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager()
                .AddDefaultTokenProviders();


            services.TryAddScoped<IUserService, UserService>();
            services.TryAddScoped<ILoginService, LoginService>();
            services.TryAddScoped<IBranchService, BranchService>();
            services.TryAddScoped<IEmployeeService, EmployeeService>();
            services.TryAddScoped<IAddressService, AddressService>();
            return services;

        }
    }
}
