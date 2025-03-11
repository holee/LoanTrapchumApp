namespace Applications.Requests.Transactions.OpenBranches;

public class OpenBranchTransactionDto
{
    public int TranTypeRefId { get; set; }
    public string? TransactionTypeName { get; set; } 
    public long? UserRefId { get; set; }

    public OpenBranchDtoCreate OpenBranch { get; set; } = new();

}
