using Applications.Requests.Transactions.ForiegnExchanges;
using Applications.Requests.Transactions.LoanTransactions;
namespace Applications.Requests.Loans;

public sealed class LoanRepaymentDtoCreate
{
    public long? TranxId { get; set; }
    public long? LoanRefId { get; set; }
    public long? CreditAgentRefId { get; set; }
    public long? UserRefId { get; set; }
    public int? BranchRefId { get; set; }
    public int? CurrencyRefId { get; set; }
    public string? OfficialReceiptNo { get; set; }
    public DateTime? PayDate { get; set; }
    public int? InstallmentNo { get; set; }
    public decimal? Principle { get; set; }
    public decimal? Interest { get; set; }
    public decimal? CollectionFeeAmount { get; set; }
    public decimal? PenaltyAmount { get; set; }
    public decimal? SubTotal { get; set; }
    public LoanTransactionDtoCreate? LoanTransaction { get; set; } = new();
    public TrxForeignExchangeDtoCreate? ForeignExchangeTrx { get; set; } = new();
    public ICollection<LoanRepaymentLineDtoCreate> LoanRepaymentLines { get; set; } = [];
}
