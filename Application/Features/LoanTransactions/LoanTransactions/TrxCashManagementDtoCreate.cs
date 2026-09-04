namespace Application.Features.Transactions.LoanTransactions;

public class TrxCashManagementDtoCreate
{

    public string? Id { get; set; } 
    public int? BankRefId { get; set; }
    public string? StaffRefId { get; set; }
    public int? CurrencyRefId { get; set; }
    public int? BranchRefId { get; set; }
    public string? TranRefId { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Balance { get; set; }
    public int? CashStatusRefId { get; set; }
    public DateTime? TransactionDate { get; set; }
    public DateTime? BusinessDate { get; set; }
    public string? Reject { get; set; } = "N";
    public string? Receive { get; set; } = "Y";
    public string? Remark { get; set; }
}
