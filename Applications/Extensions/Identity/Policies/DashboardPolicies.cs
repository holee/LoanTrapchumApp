using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public partial class Policies 
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
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Resources.BranchesDashboard, Actions.Read))
                    .Build();
    }

    public static AuthorizationPolicy CanShowLoanDashboard()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Resources.LoansDashboard,  Actions.Read))
                    .Build();
    }

    public static AuthorizationPolicy CanShowAccountDashboard()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Resources.AccountsDashboard, Actions.Read))
                    .Build();
    }


    public static AuthorizationPolicy CanShowReportDashboard()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Resources.ReportsDashboard, Actions.Read))
                    .Build();
    }

    public static AuthorizationPolicy CanShowAdminDashboard()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Resources.AdministratorsDashboard, Actions.Read))
                    .Build();
    }

}
