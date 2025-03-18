using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies
{
    public static class LoanPolicies
    {
        private static string read = AppPermission.RuleFor(Modules.Loans, Feature.Read);
        private static string create = AppPermission.RuleFor(Modules.Loans, Feature.Create);
        private static string update = AppPermission.RuleFor(Modules.Loans, Feature.Update);
        private static string delete = AppPermission.RuleFor(Modules.Loans, Feature.Delete);
        public static AuthorizationPolicy CanRead()
        {
            return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(AppClaim.Permission, read)
                        .Build();

        }

        public static AuthorizationPolicy CanCreate()
        {
            return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(AppClaim.Permission, create)
                        .Build();

        }
        public static AuthorizationPolicy CanUpdate()
        {
            return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(AppClaim.Permission, update)
                        .Build();

        }
        public static AuthorizationPolicy CanDelete()
        {
            return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(AppClaim.Permission, delete)
                        .Build();

        }

        //public static AuthorizationPolicy CanDisburseLoan()
        //{
        //    return new AuthorizationPolicyBuilder()
        //                .RequireAuthenticatedUser()
        //                .RequireRole(AppRoles.Account, AppRoles.SeniorAccount, AppRoles.ChiefAccount)
        //                .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Modules.Loans, Feature.Disburse))
        //                .Build();
        //}




    }






}
