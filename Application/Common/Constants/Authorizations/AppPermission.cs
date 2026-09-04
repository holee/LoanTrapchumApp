namespace Application.Constants.Authorizations;


public record AppPermission(string Resource, string Action, string Group, string Description, bool IsBasic = false)
{
    public string For => RuleFor(Resource,Action);
    public static string RuleFor(string resource,string action)
    {
        return $"Permissions.{resource}.{action}";
    }
}

