using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public partial class Policies
{
    private const string cust_prefix = "customers";
    public const string CreateCustomer = $"{cust_prefix}.{nameof(CreateCustomer)}";
    public const string ReadCustomer = $"{cust_prefix}.{nameof(ReadCustomer)}";
    public const string UpdateCustomer = $"{cust_prefix}.{nameof(UpdateCustomer)}";
    public const string DeleteCustomer = $"{cust_prefix}.{nameof(DeleteCustomer)}";

    private static string read_customer = AppPermission.RuleFor(Modules.Customers, Actions.Read);
    private static string update_customer = AppPermission.RuleFor(Modules.Customers, Actions.Update);
    private static string create_customer = AppPermission.RuleFor(Modules.Customers, Actions.Create);
    private static string delete_customer = AppPermission.RuleFor(Modules.Customers, Actions.Delete);
    public static AuthorizationPolicy CanCreateCustomer()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, create_customer)
                    .Build();
    }

    public static AuthorizationPolicy CanUpdateCustomer() 
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                     .RequireClaim(AppClaim.Permission, update_customer)
                    .Build();
    }


    public static AuthorizationPolicy CanReadCustomer()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, read_customer)
                    .Build();
    }


    public static AuthorizationPolicy CanDeleteCustomer()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, delete_customer)
                    .Build();
    }

}
