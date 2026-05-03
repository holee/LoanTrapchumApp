using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies
{
    public partial class Policies
    {
        private const string loan_prefix = "loans";
        public const string CreateLoan = $"{loan_prefix}.{nameof(CreateLoan)}";
        public const string UpdateLoan = $"{loan_prefix}.{nameof(UpdateLoan)}";
        public const string DeleteLoan = $"{loan_prefix}.{nameof(DeleteLoan)}";
        public const string ReadLoan = $"{loan_prefix}.{nameof(ReadLoan)}";

        private static string read_loan = AppPermission.RuleFor(Modules.Loans, Actions.Read);
        private static string create_loan = AppPermission.RuleFor(Modules.Loans, Actions.Create);
        private static string update_loan = AppPermission.RuleFor(Modules.Loans, Actions.Update);
        private static string delete_loan = AppPermission.RuleFor(Modules.Loans, Actions.Delete);
        public static AuthorizationPolicy CanReadLoan()
        {
            return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(AppClaim.Permission, read_loan)
                        .Build();

        }

        public static AuthorizationPolicy CanCreateLoan()
        {
            return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(AppClaim.Permission, create_loan)
                        .Build();

        }
        public static AuthorizationPolicy CanUpdateLoan()
        {
            return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(AppClaim.Permission, update_loan)
                        .Build();

        }
        public static AuthorizationPolicy CanDeleteLoan()
        {
            return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(AppClaim.Permission, delete_loan)
                        .Build();

        }

    }






}
