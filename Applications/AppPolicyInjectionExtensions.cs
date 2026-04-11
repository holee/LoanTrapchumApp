using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Policies;
using Microsoft.AspNetCore.Authorization;
namespace Applications
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

            options.AddPolicy(Policies.PayOff, Policies.CanPayOff());
            options.AddPolicy(Policies.WriteOff, Policies.CanWriteOff());
            options.AddPolicy(Policies.MultiplePayment, Policies.CanPayMultiple());
            options.AddPolicy(Policies.Create, Policies.CanCreate());
            options.AddPolicy(Policies.Read, Policies.CanRead());
            options.AddPolicy(Policies.Update, Policies.CanUpdate());
            options.AddPolicy(Policies.Delete, Policies.CanDelete());
            options.AddPolicy(Policies.RecievePayment, Policies.CanRecievePayment());
            options.AddPolicy(Policies.ReadLoanRequest, Policies.CanReadLoanRequest());
            options.AddPolicy(Policies.CreateLoanRequest, Policies.CanCreateLoanRequest());
            options.AddPolicy(Policies.RequestLoanRequest, Policies.CanRequestLoanRequest());
            options.AddPolicy(Policies.UpdateLoanRequest, Policies.CanUpdateLoanRequest());
            options.AddPolicy(Policies.DeleteLoanRequest, Policies.CanDeleteLoanRequest());
            options.AddPolicy(Policies.ApproveLoanRequest, Policies.CanAproveRequest());
            options.AddPolicy(Policies.RejectLoanRequest, Policies.CanRejectRequest());
            options.AddPolicy(Policies.Disburse, Policies.CanDisburseLoan());
            options.AddPolicy(Policies.ReadLoan, Policies.CanReadLoan());
            options.AddPolicy(Policies.CreateLoan, Policies.CanCreateLoan());
            options.AddPolicy(Policies.UpdateLoan, Policies.CanUpdateLoan());
            options.AddPolicy(Policies.DeleteLoan, Policies.CanDeleteLoan());
            options.AddPolicy(Policies.ShowBranchDashboard, Policies.CanShowBranchDashboard());
            options.AddPolicy(Policies.ShowLoanDashboard, Policies.CanShowLoanDashboard());
            options.AddPolicy(Policies.ShowAccountDashboard, Policies.CanShowAccountDashboard());
            options.AddPolicy(Policies.ShowReportDashboard, Policies.CanShowReportDashboard());
            options.AddPolicy(Policies.ShowAdminDashboard, Policies.CanShowAdminDashboard());
            options.AddPolicy(Policies.AdminOnly, Policies.IsAdminOnly());
            options.AddPolicy(Policies.AccountOnly, Policies.IsAccountOnly());
            options.AddPolicy(Policies.CEOOnly, Policies.IsCEOOnly());
            options.AddPolicy(Policies.CreditOfficerOnly, Policies.IsCreditOfficerOnly());
            options.AddPolicy(Policies.ManagerOnly, Policies.IsManagerOnly());
            options.AddPolicy(Policies.ReadCustomer, Policies.CanReadCustomer());
            options.AddPolicy(Policies.CreateCustomer, Policies.CanCreateCustomer());
            options.AddPolicy(Policies.UpdateCustomer, Policies.CanUpdateCustomer());
            options.AddPolicy(Policies.DeleteCustomer, Policies.CanDeleteCustomer());
            options.AddPolicy(Policies.ReadEmployee, Policies.CanReadEmployee());
            options.AddPolicy(Policies.CreateEmployee, Policies.CanCreateEmployee());
            options.AddPolicy(Policies.UpdateEmployee, Policies.CanEditEmployee());
            options.AddPolicy(Policies.DeleteEmployee, Policies.CanDeleteEmployee());
            options.AddPolicy(Policies.BranchRead, Policies.CanReadBranch());
            options.AddPolicy(Policies.BranchCreate, Policies.CanCreateBranch());
            options.AddPolicy(Policies.BranchUpdate, Policies.CanUpdateBranch());
            options.AddPolicy(Policies.BranchDelete, Policies.CanDeleteBranch());
            options.AddPolicy(Policies.BranchShowBranch, Policies.CanDeleteBranch());
            options.AddPolicy(Policies.TranxRead, Policies.TranxCanRead());
            options.AddPolicy(Policies.TranxCreate, Policies.TranxCanCreate());
            options.AddPolicy(Policies.TranxUpdate, Policies.TranxCanUpdate());
            options.AddPolicy(Policies.TranxDelete, Policies.TranxCanDelete());
            options.AddPolicy(Policies.TranxVaultOut, Policies.CanVaultOut());
            options.AddPolicy(Policies.TranxVaultIn, Policies.CanVaultIn());
            options.AddPolicy(Policies.OpenBranch, Policies.CanOpenBranch());
            options.AddPolicy(Policies.CloseBranch, Policies.CanClosedBranch());
            options.AddPolicy(Policies.CanExchanged, Policies.CanExchangeMoney());

        }

        public static IServiceCollection AddPolicyServices(this IServiceCollection services)
        {
            services.AddTransientServices();
            services.AddAuthorizationCore(option => RegisterPermissionClaims(option));
            return services;
        }
    }
}
