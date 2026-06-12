using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public partial class Policies
{
    private const string emp_prefix = "employees";
    public const string CreateEmployee = $"{emp_prefix}.{nameof(CreateEmployee)}";
    public const string ReadEmployee = $"{emp_prefix}.{nameof(ReadEmployee)}";
    public const string UpdateEmployee = $"{emp_prefix}.{nameof(UpdateEmployee)}";
    public const string DeleteEmployee = $"{emp_prefix}.{nameof(DeleteEmployee)}";
    private static string read_employee = AppPermission.RuleFor(Resources.Employees, Actions.Read);
    private static string update_employee = AppPermission.RuleFor(Resources.Employees, Actions.Update);
    private static string create_employee = AppPermission.RuleFor(Resources.Employees, Actions.Create);
    private static string delete_employee = AppPermission.RuleFor(Resources.Employees, Actions.Delete);
    public static AuthorizationPolicy CanCreateEmployee()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, create_employee)
                    .Build();
    }

    public static AuthorizationPolicy CanEditEmployee()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                     .RequireClaim(AppClaim.Permission, update_employee)
                    .Build();
    }

    public static AuthorizationPolicy CanReadEmployee()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, read_employee)
                    .Build();
    }

    public static AuthorizationPolicy CanDeleteEmployee()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, delete_employee)
                    .Build();
    }

}
