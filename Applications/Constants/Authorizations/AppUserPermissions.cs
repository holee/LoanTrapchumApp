namespace Applications.Constants.Authorizations
{
    public record AppUserPermissions(string Group, string Action)
    {
        public string Name => NameFor(Group, Action);
        public static string NameFor(string feature, string action)
        {
            return $"Permissions.{feature}.{action}";
        }

    }
}
