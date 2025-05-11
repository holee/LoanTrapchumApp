namespace Applications.Requests.Transactions.OpenBranches;

public class CloseBranchDtoUpdate
{

    public string? OpenBranchId { get; set; }
    public int? BankRefId { get; set; }
    public int? BranchRefId { get; set; }
    
    public long? LoginLogId { get; set; }
    public DateTime? OpenDate { get; set; }
    public string? OpenStatus { get; set; } = "N";
}
