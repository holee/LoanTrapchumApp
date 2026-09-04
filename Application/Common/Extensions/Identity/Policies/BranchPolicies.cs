namespace Application.Extensions.Identity.Policies;
public class BranchPolicies
{
    private const string prefix = "branches";
    public const string Create = $"{prefix}.{nameof(Create)}";
    public const string Read = $"{prefix}.{nameof(Read)}";
    public const string Update = $"{prefix}.{nameof(Update)}";
    public const string Delete = $"{prefix}.{nameof(Delete)}";
    public const string ShowBranch = $"{prefix}.{nameof(ShowBranch)}";
}
