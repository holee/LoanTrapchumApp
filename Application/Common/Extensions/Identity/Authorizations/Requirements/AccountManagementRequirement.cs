using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Authorizations.Requirements
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
