namespace Applications.Requests.Transactions.OpenBranches;

public class OpenBranchDtoCreate
{
    public int? BankRefId { get; set; }
    public int? BranchRefId { get; set; }
    public long? LoginLogId { get; set; }
    public long? DelegateStaffId { get; set; }
    public DateTime? OpenDate { get; set; }
    public string? OpenStatus { get; set; }
    public OpenBranchTransactionDto? LoanTransaction { get; set; }
}
