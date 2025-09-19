namespace Applications.Features.Transactions.ForiegnExchanges;


public abstract class TransactionBase 
{
    public int TranTypeRefId { get; set; }
    public string? TransactionTypeName { get; set; }
    public string? UserRefId { get; set; }
    public int BranchRefId { get; set; }= 0;
}


public class TrxForeignExchangeDtoCreate : TransactionBase
{
    public ICollection<ForeignExchangeDtoCreate>? ForeignExchanges { get; set; } = [];
}
