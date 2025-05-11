using Applications.Requests.Transactions.ForiegnExchanges;
using Applications.Requests.Transactions.LoanTransactions;
namespace Applications.Requests.Loans.Payments;

public sealed class LoanRepaymentDtoCreate
{
    public string? TranxId { get; set; }
    public string? LoanRefId { get; set; }
    public string? CreditAgentRefId { get; set; }
    public string? UserRefId { get; set; }
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
