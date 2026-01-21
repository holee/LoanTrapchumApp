using Domains.Models.Bases;
using Domains.Models.Journals;
using Domains.Models.Loans;

namespace Domains.Models.Transaction;

public sealed class LoanTransaction : BaseEntity
{
    public string TransactionsId { get; set; } = $"trx-{Guid.CreateVersion7()}";
    public string? UserRefId { get; set; }
    public int? BranchRefId { get; set; }
    public int? TranTypeRefId { get; set; }
    public string? TransactionTypeName { get; set; } = string.Empty;
    public DateTimeOffset? TransactionDate { get; set; } = DateTimeOffset.Now;
    public bool? IsActive { get; set; } = true;
    public JournalEntry? JournalEntry { get; set; }
    public OpenBranch? OpenBranch { get; set; }
    public CashVault? CashVault { get; set; }
    public LoanPayment? LoanPayment { get; set; }
    public ForeignExchange? ForeignExchanges { get; set; }
    public List<CashManagement> CashManagements { get; set; } = [];

}
