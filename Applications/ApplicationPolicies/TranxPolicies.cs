namespace Applications.ApplicationPolicies;

public class TranxPolicies 
{
    private const string prefix = "tranx";
    public const string Create = $"{prefix}.{nameof(Create)}";
    public const string Read = $"{prefix}.{nameof(Read)}";
    public const string Update = $"{prefix}.{nameof(Update)}";
    public const string Delete = $"{prefix}.{nameof(Delete)}";
    public const string VaultIn = $"{prefix}.{nameof(VaultIn)}";
    public const string VaultOut = $"{prefix}.{nameof(VaultOut)}";
    public const string OpenBranch = $"{prefix}.{nameof(OpenBranch)}";
    public const string CloseBranch = $"{prefix}.{nameof(CloseBranch)}";
} 
