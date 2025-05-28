namespace Applications.Requests.Transactions.OpenBranches;

public class TransactionCloseBranchDto
{
    public string? TransactionsId { get; set; } = $"trx_{Guid.CreateVersion7()}";
    public string? UserRefId { get; set; }
    public int? BranchRefId  { get; set; }
    public int? TranTypeRefId { get; set; } 
    public string? TransactionTypeName { get; set; }
    public CloseBranchDtoUpdate OpenBranch { get; set; } = new();

}
