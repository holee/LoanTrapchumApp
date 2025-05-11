namespace Applications.Requests.Transactions.LoanTransactions;

public class LoanTransactionDtoCreate
{
    public Guid? TranCode { get; set; }
    public int? TranTypeRefId { get; set; }
    public string TransactionTypeName { get; set; } = string.Empty;
    public DateTime? TransactionDate { get; set; } = DateTime.Now;
    public string? UserRefId { get; set; }
    public ICollection<TrxCashManagementDtoCreate>? CashManagements { get; set; } = [];
}
