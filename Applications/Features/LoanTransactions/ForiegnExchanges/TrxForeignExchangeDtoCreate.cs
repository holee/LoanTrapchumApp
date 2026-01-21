namespace Applications.Features.Transactions.ForiegnExchanges;


public class TransactionBase
{
    public int TranTypeRefId { get; set; }
    public string? TransactionTypeName { get; set; }
    public string? UserRefId { get; set; }
    public int BranchRefId { get; set; } = 0;
    public DateTimeOffset? TransactionDate { get; set; } = DateTimeOffset.UtcNow;
    public string? TransactionId { get; set; }
}


public class TrxForeignExchangeDtoCreate : TransactionBase
{
    public ForeignExchangeDtoCreate? ForeignExchanges { get; set; } = new();
}
