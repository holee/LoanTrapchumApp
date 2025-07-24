namespace Applications.Constants.Authorizations;

public record PermissionDto(string Module, string Action,string Description) 
{
    public string For => RuleFor(Module, Action);
    public static string RuleFor(string module, string action) 
    {
        return $"Permissions.{module}.{action}";
    }
}
