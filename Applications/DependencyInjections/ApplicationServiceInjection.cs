using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Policies;
using Applications.Interfaces.Addresses;
using Applications.Interfaces.Banks;
using Applications.Interfaces.Branches;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.Customers;
using Applications.Interfaces.Employees;
using Applications.Interfaces.Identity;
using Applications.Interfaces.Lists;
using Applications.Interfaces.setting;
using Applications.Interfaces.Transactions;
using Applications.Services.Addresses;
using Applications.Services.Banks;
using Applications.Services.Branches;
using Applications.Services.Currencies;
using Applications.Services.Customers;
using Applications.Services.Employees;
using Applications.Services.Identity;
using Applications.Services.Lists;
using Applications.Services.Settings;
using Applications.Services.Transactions.Branches;
using Applications.Services.Transactions.Currencies;
using Applications.Services.Transactions.Journals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static Applications.ApplicationPolicies.Policies;
namespace Applications.DependencyInjections
{
    public interface ITransient
    {
        //void InterceptAfterHttp(object sender, HttpClientInterceptorEventArgs e);
    }
    public static class ApplicationServiceInjection
    {
        internal static void RegisterPermissionClaims(AuthorizationOptions options)
        {
            foreach (var permission in ApplicationPermissions.AllPermissions)
            {
                options.AddPolicy(permission.For, policy => policy.RequireClaim(AppClaim.Permission, permission.For));
            }
            foreach (var permission in UserLevelPermissions.AllUserPermissions)
            {
                options.AddPolicy(permission.Name, policy => policy.RequireClaim(AppClaim.Permission, permission.Name));
            }

            options.AddPolicy(RepaymentPolicies.PayOff, PaymentsPolicies.CanPayOff());
            options.AddPolicy(RepaymentPolicies.MultiplePayment, PaymentsPolicies.CanPayMultiple());
            options.AddPolicy(RepaymentPolicies.Create, PaymentsPolicies.CanCreate());
            options.AddPolicy(RepaymentPolicies.Read, PaymentsPolicies.CanRead());
            options.AddPolicy(RepaymentPolicies.Update, PaymentsPolicies.CanUpdate());
            options.AddPolicy(RepaymentPolicies.Delete, PaymentsPolicies.CanDelete());
            options.AddPolicy(LoanRequestsPolicies.Read, LoanRequestPolicies.CanRead());
            options.AddPolicy(LoanRequestsPolicies.Create, LoanRequestPolicies.CanCreate());
            options.AddPolicy(LoanRequestsPolicies.Request, LoanRequestPolicies.CanRequest());
            options.AddPolicy(LoanRequestsPolicies.Update, LoanRequestPolicies.CanUpdate());
            options.AddPolicy(LoanRequestsPolicies.Delete, LoanRequestPolicies.CanDelete());
            options.AddPolicy(LoanRequestsPolicies.Approve, LoanRequestPolicies.CanAproveRequest());
            options.AddPolicy(LoanRequestsPolicies.Reject, LoanRequestPolicies.CanRejectRequest());
            options.AddPolicy(LoanRequestsPolicies.Disburse, LoanRequestPolicies.CanDisburseLoan());
            options.AddPolicy(LoansPolicies.Read, LoanPolicies.CanRead());
            options.AddPolicy(LoansPolicies.Create, LoanPolicies.CanCreate());
            options.AddPolicy(LoansPolicies.Update, LoanPolicies.CanUpdate());
            options.AddPolicy(LoansPolicies.Delete, LoanPolicies.CanDelete());
            options.AddPolicy(DashboardsPolicies.ShowBranchDashboard, DashboardPolicies.CanShowBranchDashboard());
            options.AddPolicy(DashboardsPolicies.ShowLoanDashboard, DashboardPolicies.CanShowLoanDashboard());
            options.AddPolicy(DashboardsPolicies.ShowAccountDashboard, DashboardPolicies.CanShowAccountDashboard());
            options.AddPolicy(DashboardsPolicies.ShowReportDashboard, DashboardPolicies.CanShowReportDashboard());
            options.AddPolicy(DashboardsPolicies.ShowAdminDashboard, DashboardPolicies.CanShowAdminDashboard());
            options.AddPolicy(AdminOnly, AppPolicies.IsAdminOnly());
            options.AddPolicy(AccountOnly, AppPolicies.IsAccountOnly());
            options.AddPolicy(CreditOfficerOnly, AppPolicies.IsCreditOfficerOnly());
            options.AddPolicy(ManagerOnly, AppPolicies.IsManagerOnly());
            options.AddPolicy(CustomerPolicies.Read, CustomersPolicies.CanRead());
            options.AddPolicy(CustomerPolicies.Create, CustomersPolicies.CanCreate());
            options.AddPolicy(CustomerPolicies.Update, CustomersPolicies.CanUpdate());
            options.AddPolicy(CustomerPolicies.Delete, CustomersPolicies.CanDelete());
            options.AddPolicy(EmployeePolicies.Read, CustomersPolicies.CanRead());
            options.AddPolicy(EmployeePolicies.Create, CustomersPolicies.CanCreate());
            options.AddPolicy(EmployeePolicies.Update, CustomersPolicies.CanUpdate());
            options.AddPolicy(EmployeePolicies.Delete, CustomersPolicies.CanDelete());
            options.AddPolicy(BranchPolicies.Read, BranchesPolicies.CanRead());
            options.AddPolicy(BranchPolicies.Create, BranchesPolicies.CanCreate());
            options.AddPolicy(BranchPolicies.Update, BranchesPolicies.CanUpdate());
            options.AddPolicy(BranchPolicies.Delete, BranchesPolicies.CanDelete());
            options.AddPolicy(BranchPolicies.ShowBranch, BranchesPolicies.CanDelete());
            options.AddPolicy(TranxPolicies.Read, TranxsPolicies.CanRead());
            options.AddPolicy(TranxPolicies.Create, TranxsPolicies.CanCreate());
            options.AddPolicy(TranxPolicies.Update, TranxsPolicies.CanUpdate());
            options.AddPolicy(TranxPolicies.Delete, TranxsPolicies.CanDelete());
            options.AddPolicy(TranxPolicies.VaultOut, TranxsPolicies.CanVaultOut());
            options.AddPolicy(TranxPolicies.VaultIn, TranxsPolicies.CanVaultIn());
            options.AddPolicy(TranxPolicies.OpenBranch, TranxsPolicies.CanOpenBranch());
            options.AddPolicy(TranxPolicies.CloseBranch, TranxsPolicies.CanClosedBranch());

        }
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

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.TryAddTransient<IAddressService, AddressService>();
            services.TryAddTransient<IBranchService, BranchService>();
            services.TryAddTransient<IBankService, BankService>();
            services.TryAddTransient<IOpenBranchService, OpenBranchService>();
            services.TryAddTransient<ICurrencyService, CurrencyService>();
            services.TryAddTransient<ICustomerService, CustomerService>();
            services.TryAddTransient<ICustomerIncomeExpendService, CustomerIncomeExpendService>();
            services.TryAddTransient<ICreditAgentService, CreditAgentService>();
            services.TryAddTransient<IEmployeeService, EmployeeService>();
            services.TryAddTransient<IDepartmentService, DepartmentService>();
            services.TryAddTransient<IEntityNameService, EntityNameService>();
            services.TryAddTransient<ICreditAgentService, CreditAgentService>();
            services.TryAddTransient<ILoginService, LoginService>();
            services.TryAddTransient<IUserService, UserService>();
            services.TryAddTransient<IForiegnCurrencyExchange, ForiegnCurrencyExchange>();
            services.TryAddTransient<IJournalService, JournalService>();
            services.TryAddTransient<ILedgerService, LedgerService>();
            services.TryAddTransient<IRoleService, RoleService>();
            services.TryAddTransient<ISpotRateService, SpotRateService>();
            services.TryAddTransient<ISettingService, SettingService>();
            services.AddAuthorizationCore(option => RegisterPermissionClaims(option));
            return services;

        }
    }
}
