
using Domains.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Extensions.Identity.Authorizations.Services
{
    public class RoleServiceImpl : IRoleServiceImpl 
    { 
        private readonly RoleManager<ApplicationRole> roleManager;

        public RoleServiceImpl(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public HashSet<string> GetRolesAsync()
        {
            HashSet<string?> roles =roleManager.Roles
                                    .Select(x => x.Name)
                                    .ToHashSet();
            return roles!;
        }
    }
}
