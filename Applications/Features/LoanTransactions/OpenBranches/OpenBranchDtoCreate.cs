namespace Applications.Features.Transactions.OpenBranches;

public sealed class OpenBranchDtoCreate
{
    
    public string? OpenBranchId { get; set; }
    public int? BankRefId { get; set; }
    public int? BranchRefId { get; set; }   
    public long? LoginLogId { get; set; } 
    public long? DelegateStaffId { get; set; }
    public DateTime? OpenDate { get; set; }
    public int OpenStatus { get; set; } = 0;
    //public OpenBranchTransactionDto? LoanTransaction { get; set; }
}
