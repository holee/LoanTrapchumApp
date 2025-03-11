namespace Applications.Requests.Transactions.OpenBranches;

public class TransactionCloseBranchDto 
{
    public int? TranTypeRefId { get; set; } 
    public string? TransactionTypeName { get; set; } 
    public long? UserRefId { get; set; }
    public CloseBranchDtoUpdate OpenBranch { get; set; } = new();

}
