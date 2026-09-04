using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Authorizations.AuthorizeRequirements
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
        public string Permission { get; }
    }
}
