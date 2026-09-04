namespace Application.Extensions.Identity.Policies;
public partial class Policies
{

    private const string role_prefix = "roles";
    public const string CreateRole = $"{role_prefix}.{nameof(CreateRole)}";
    public const string ReadRole = $"{role_prefix}.{nameof(ReadRole)}";
    public const string UpdateRole = $"{role_prefix}.{nameof(UpdateRole)}";
    public const string DeleteRole = $"{role_prefix}.{nameof(DeleteRole)}";
    public const string GrantRole = $"{role_prefix}.{nameof(GrantRole)}";

    public const string AdminOnly = nameof(AdminOnly);
    public const string BMOnly = nameof(BMOnly);
    public const string AccountOnly = nameof(AccountOnly);
    public const string CreditOfficerOnly = nameof(CreditOfficerOnly);
    public const string ManagerOnly = nameof(ManagerOnly);
    public const string CEOOnly = nameof(CEOOnly);
    public const string SameUser = nameof(SameUser);
}
