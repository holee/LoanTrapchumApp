namespace Application.Features.Transactions.OpenBranches;

public class OpenBranchTransactionDto
{
    public string? TransactionsId { get; set; } = $"trx_{Guid.CreateVersion7()}";
    public int TranTypeRefId { get; set; }
    public string? TransactionTypeName { get; set; } 
    public string? UserRefId { get; set; }
    public int? BranchRefId { get; set; }
    public OpenBranchDtoCreate OpenBranch { get; set; } = new();

}
