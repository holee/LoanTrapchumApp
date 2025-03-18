using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Authorizations.Requirements
{
    public class AccountDashboardManagementRequirement : IAuthorizationRequirement
    {
        public AccountDashboardManagementRequirement(string permission)
        {
            Permission = permission;
        }

        public string Permission { get; }
    }
}
