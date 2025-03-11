namespace Applications.ApplicationPolicies;

public static partial class Policies
{
    public static class UserPolicies
    {
        private const string prefix = "users";
        public const string Create = $"{prefix}.{nameof(Create)}";
        public const string Read = $"{prefix}.{nameof(Read)}";
        public const string Update = $"{prefix}.{nameof(Update)}";
        public const string Delete = $"{prefix}.{nameof(Delete)}";
        public const string Grant = $"{prefix}.{nameof(Grant)}";
    }
}
