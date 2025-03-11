using Microsoft.AspNetCore.Authorization;

namespace Applications.ApplicationPolicies
{
    public static class CreatePolicy
    {
        public const string CreateRolePolicy = "CanCreateRolePolicy";
        public const string CreateUserPolicy = "CanCreateUserPolicy";
        public const string CreateEmployeePolicy = "CanCreateEmployeePolicy";
        public const string CreateCustomerPolicy = "CanCreateCustomerPolicy";
        public const string CreateLoanDisbursmentPolicy = "CanCreateLoanDisbursmentPolicy";
        public static AuthorizationPolicy CanCreateRolePolicy()
        {
            return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim("", "")
                        .Build();

        }

    }




    public static class AproveLoanPolicies 
    {
        public const string CreateRolePolicy = "CanCreateRolePolicy";
        public static AuthorizationPolicy CanCreateRolePolicy()
        {
            return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim("", "")
                        .Build();

        }

    }

}
