namespace Applications.ApplicationPolicies;

public static partial class Policies
{
    public class RolePolicies
    {
        private const string prefix = "roles";
        public const string Create = $"{prefix}.{nameof(Create)}";
        public const string Read = $"{prefix}.{nameof(Read)}";
        public const string Update = $"{prefix}.{nameof(Update)}";
        public const string Delete = $"{prefix}.{nameof(Delete)}";
        public const string Grant = $"{prefix}.{nameof(Grant)}";
    }
}
