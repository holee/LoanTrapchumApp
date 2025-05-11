using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public static class DashboardPolicies 
{
    public const string ShowBranchDashboard = nameof(ShowBranchDashboard);
    public const string ShowLoanDashboard = nameof(ShowLoanDashboard);
    public const string ShowAccountDashboard = nameof(ShowAccountDashboard);
    public const string ShowReportDashboard = nameof(ShowReportDashboard);
    public const string ShowOperatedDashboard = nameof(ShowOperatedDashboard);
    public const string ShowAdminDashboard = nameof(ShowAdminDashboard);
    public static AuthorizationPolicy CanShowBranchDashboard()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Modules.BranchesDashboard, Feature.Read))
                    .Build();
    }

    public static AuthorizationPolicy CanShowLoanDashboard()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Modules.LoansDashboard,  Feature.Read))
                    .Build();
    }

    public static AuthorizationPolicy CanShowAccountDashboard()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Modules.AccountsDashboard, Feature.Read))
                    .Build();
    }


    public static AuthorizationPolicy CanShowReportDashboard()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Modules.ReportsDashboard, Feature.Read))
                    .Build();
    }

    public static AuthorizationPolicy CanShowAdminDashboard()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Modules.AdministratorsDashboard, Feature.Read))
                    .Build();
    }

}
