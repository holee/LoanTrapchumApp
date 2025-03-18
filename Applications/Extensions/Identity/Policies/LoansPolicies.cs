namespace Applications.Extensions.Identity.Policies;


public class LoansPolicies
{
    private const string prefix = "loans";
    public const string Create = $"{prefix}.{nameof(Create)}";
    public const string Update = $"{prefix}.{nameof(Update)}";
    public const string Delete = $"{prefix}.{nameof(Delete)}";
    public const string Read = $"{prefix}.{nameof(Read)}";

}
