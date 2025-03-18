namespace Applications.ApplicationPolicies;

public static partial class Policies
{
    public class PersonPolicies 
    {
        private const string prefix = "people";
        public const string Create = $"{prefix}.{nameof(Create)}";
        public const string Read = $"{prefix}.{nameof(Read)}";
        public const string Update = $"{prefix}.{nameof(Update)}";
        public const string Delete = $"{prefix}.{nameof(Delete)}";
    }
}
