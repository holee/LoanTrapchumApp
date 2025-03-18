namespace Applications.Extensions.Identity.Policies
{
    public static class ReadPolicy 
    {
        public const string ReadCustomerPolicy = "CanReadCustomerPolicy"; 
        //public static AuthorizationPolicy CanReadCustomerPolicy() 
        //{
        //    return new AuthorizationPolicyBuilder()
        //                .RequireAuthenticatedUser()
        //                .RequireClaim(AppClaim.Permission,AppPermission.NameFor(Modules.Customers,Features.Read))
        //                .Build();

        //}

    }

}
