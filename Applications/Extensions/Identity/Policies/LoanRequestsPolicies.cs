namespace Applications.Extensions.Identity.Policies;

public class LoanRequestsPolicies
{
    private const string prefix = "loanRequests";
    public const string Create = $"{prefix}.{nameof(Create)}";
    public const string Update = $"{prefix}.{nameof(Update)}";
    public const string Delete = $"{prefix}.{nameof(Delete)}";
    public const string Read = $"{prefix}.{nameof(Read)}";
    public const string Approve = $"{prefix}.{nameof(Approve)}";
    public const string Disburse = $"{prefix}.{nameof(Disburse)}"; 
    public const string Request = $"{prefix}.{nameof(Request)}";
    public const string Reject = $"{prefix}.{nameof(Reject)}";
}
