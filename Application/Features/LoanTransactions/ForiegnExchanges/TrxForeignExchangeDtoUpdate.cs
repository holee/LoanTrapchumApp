namespace Application.Features.Transactions.ForiegnExchanges;

public class TrxForeignExchangeDtoUpdate 
{
    public TrxForeignExchangeDtoUpdate()
    {
        ForeignExchange=new ForeignExchangeDtoUpdate();
    }
    public long? TranId { get; set; }  
    public int TranTypeRefId { get; set; }
    public string? TransactionName { get; set; }
    public long? UserRefId { get; set; }
    public ForeignExchangeDtoUpdate? ForeignExchange;
}
