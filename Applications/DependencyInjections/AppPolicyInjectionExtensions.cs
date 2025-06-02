using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
namespace Applications.DependencyInjections
{
    public static class AppPolicyInjectionExtensions
    {
        internal static IServiceCollection AddTransientServices(this IServiceCollection services)
        {
            var transientServices = typeof(ITransient);

            var types = transientServices
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null);

            foreach (var type in types)
            {
                if (transientServices.IsAssignableFrom(type.Service))
                {
                    services.AddTransient(type.Service, type.Implementation);
                }
            }

            return services;
        }

        private static void RegisterPermissionClaims(AuthorizationOptions options)
        {
            foreach (var permission in ApplicationPermissions.AllPermissions)
            {
                options.AddPolicy(permission.For, policy => policy.RequireClaim(AppClaim.Permission, permission.For));
            }
            foreach (var permission in UserLevelPermissions.AllUserPermissions)
            {
                options.AddPolicy(permission.Name, policy => policy.RequireClaim(AppClaim.Permission, permission.Name));
            }

            options.AddPolicy(PaymentsPolicies.PayOff, PaymentsPolicies.CanPayOff());
            options.AddPolicy(PaymentsPolicies.MultiplePayment, PaymentsPolicies.CanPayMultiple());
            options.AddPolicy(PaymentsPolicies.Create, PaymentsPolicies.CanCreate());
            options.AddPolicy(PaymentsPolicies.Read, PaymentsPolicies.CanRead());
            options.AddPolicy(PaymentsPolicies.Update, PaymentsPolicies.CanUpdate());
            options.AddPolicy(PaymentsPolicies.Delete, PaymentsPolicies.CanDelete());
            options.AddPolicy(RequestsPolicies.Read, RequestsPolicies.CanRead());
            options.AddPolicy(RequestsPolicies.Create, RequestsPolicies.CanCreate());
            options.AddPolicy(RequestsPolicies.Request, RequestsPolicies.CanRequest());
            options.AddPolicy(RequestsPolicies.Update, RequestsPolicies.CanUpdate());
            options.AddPolicy(RequestsPolicies.Delete, RequestsPolicies.CanDelete());
            options.AddPolicy(RequestsPolicies.Approve, RequestsPolicies.CanAproveRequest());
            options.AddPolicy(RequestsPolicies.Reject, RequestsPolicies.CanRejectRequest());
            options.AddPolicy(RequestsPolicies.Disburse, RequestsPolicies.CanDisburseLoan());
            options.AddPolicy(LoanPolicies.Read, LoanPolicies.CanRead());
            options.AddPolicy(LoanPolicies.Create, LoanPolicies.CanCreate());
            options.AddPolicy(LoanPolicies.Update, LoanPolicies.CanUpdate());
            options.AddPolicy(LoanPolicies.Delete, LoanPolicies.CanDelete());
            options.AddPolicy(DashboardPolicies.ShowBranchDashboard, DashboardPolicies.CanShowBranchDashboard());
            options.AddPolicy(DashboardPolicies.ShowLoanDashboard, DashboardPolicies.CanShowLoanDashboard());
            options.AddPolicy(DashboardPolicies.ShowAccountDashboard, DashboardPolicies.CanShowAccountDashboard());
            options.AddPolicy(DashboardPolicies.ShowReportDashboard, DashboardPolicies.CanShowReportDashboard());
            options.AddPolicy(DashboardPolicies.ShowAdminDashboard, DashboardPolicies.CanShowAdminDashboard());
            options.AddPolicy(RolePolicies.AdminOnly, AppPolicies.IsAdminOnly());
            options.AddPolicy(RolePolicies.AccountOnly, AppPolicies.IsAccountOnly());
            options.AddPolicy(RolePolicies.CreditOfficerOnly, AppPolicies.CreditOfficerOnly());
            options.AddPolicy(RolePolicies.ManagerOnly, AppPolicies.IsManagerOnly());
            options.AddPolicy(CustomersPolicies.Read, CustomersPolicies.CanRead());
            options.AddPolicy(CustomersPolicies.Create, CustomersPolicies.CanCreate());
            options.AddPolicy(CustomersPolicies.Update, CustomersPolicies.CanUpdate());
            options.AddPolicy(CustomersPolicies.Delete, CustomersPolicies.CanDelete());
            options.AddPolicy(EmployeesPolicies.Read, EmployeesPolicies.CanRead());
            options.AddPolicy(EmployeesPolicies.Create, EmployeesPolicies.CanCreate());
            options.AddPolicy(EmployeesPolicies.Update, EmployeesPolicies.CanEdit());
            options.AddPolicy(EmployeesPolicies.Delete, EmployeesPolicies.CanDelete());
            options.AddPolicy(BranchesPolicies.Read, BranchesPolicies.CanRead());
            options.AddPolicy(BranchesPolicies.Create, BranchesPolicies.CanCreate());
            options.AddPolicy(BranchesPolicies.Update, BranchesPolicies.CanUpdate());
            options.AddPolicy(BranchesPolicies.Delete, BranchesPolicies.CanDelete());
            options.AddPolicy(BranchesPolicies.ShowBranch, BranchesPolicies.CanDelete());
            options.AddPolicy(TranxsPolicies.Read, TranxsPolicies.CanRead());
            options.AddPolicy(TranxsPolicies.Create, TranxsPolicies.CanCreate());
            options.AddPolicy(TranxsPolicies.Update, TranxsPolicies.CanUpdate());
            options.AddPolicy(TranxsPolicies.Delete, TranxsPolicies.CanDelete());
            options.AddPolicy(TranxsPolicies.VaultOut, TranxsPolicies.CanVaultOut());
            options.AddPolicy(TranxsPolicies.VaultIn, TranxsPolicies.CanVaultIn());
            options.AddPolicy(TranxsPolicies.OpenBranch, TranxsPolicies.CanOpenBranch());
            options.AddPolicy(TranxsPolicies.CloseBranch, TranxsPolicies.CanClosedBranch());

        }

        public static IServiceCollection AddPolicyServices(this IServiceCollection services)
        {
            services.AddTransientServices();
            services.AddAuthorizationCore(option => RegisterPermissionClaims(option));
            return services;
        }
    }
}
