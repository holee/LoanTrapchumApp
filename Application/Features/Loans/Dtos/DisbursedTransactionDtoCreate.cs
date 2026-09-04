namespace Application.Features.Loans.Dtos;

public sealed class DisbursedTransactionDtoCreate 
{
    public string TransactionsId { get; set; } = $"trx-{Guid.CreateVersion7()}";
    public string? UserRefId { get; set; }
    public int? BranchRefId { get; set; }
    public int? TranTypeRefId { get; set; }
    public string? TransactionTypeName { get; set; } = string.Empty;
    public DateTimeOffset? TransactionDate { get; set; } = DateTimeOffset.Now;
    public bool? IsActive { get; set; } = true;
}
