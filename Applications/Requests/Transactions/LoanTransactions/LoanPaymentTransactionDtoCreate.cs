using Applications.Requests.Loans.Payments;

namespace Applications.Requests.Transactions.LoanTransactions;

public sealed class LoanPaymentTransactionDtoCreate
{
    public string TransactionsId { get; set; } = string.Empty;
    public int? TranTypeRefId { get; set; }
    public int? BranchRefId { get; set; } 
    public string TransactionTypeName { get; set; } = string.Empty;
    public DateTime? TransactionDate { get; set; } = DateTime.Now;
    public string? UserRefId { get; set; }
    public LoanPaymentDtoCreate? LoanPayment { get; set; }
    public ICollection<TrxCashManagementDtoCreate>? CashManagements { get; set; } = [];
}
