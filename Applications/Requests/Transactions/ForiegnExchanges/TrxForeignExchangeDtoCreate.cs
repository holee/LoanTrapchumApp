namespace Applications.Requests.Transactions.ForiegnExchanges;


public abstract class TransactionBase 
{
    public int TranTypeRefId { get; set; }
    public string? TransactionTypeName { get; set; }
    public long? UserRefId { get; set; }
}


public class TrxForeignExchangeDtoCreate : TransactionBase
{
    public ICollection<ForeignExchangeDtoCreate>? ForeignExchanges { get; set; } = [];
}
