namespace Applications.ApplicationPolicies;

public static class RepaymentPolicies
{
    private const string prefix = "repayments";
    public const string Create = $"{prefix}.{nameof(Create)}";
    public const string Update = $"{prefix}.{nameof(Update)}";
    public const string Delete = $"{prefix}.{nameof(Delete)}";
    public const string Read = $"{prefix}.{nameof(Read)}";
    public const string MultiplePayment = $"{prefix}.{nameof(MultiplePayment)}";
    public const string PayOff = $"{prefix}.{nameof(PayOff)}";
}

