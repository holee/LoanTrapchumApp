namespace Applications.Constants.Authorizations;


public record AppPermission(string Module, string Action, string Group, string Description, bool IsBasic = false)
{
    public string For => RuleFor(Module,Action);
    public static string RuleFor(string module,string action)
    {
        return $"Permissions.{module}.{action}";
    }
}

