using Domains.Models.Bases;
using Domains.Models.Journals;

namespace Domains.Models.Transaction;

public class LoanTransaction : BaseEntity
{
    public string TransactionsId { get; set; } =$"trx-{Guid.CreateVersion7()}";
    public int? TranTypeRefId { get; set; }
    public string TransactionTypeName { get; set; } = string.Empty;
    public DateTime? TransactionDate { get; set; } = DateTime.Now;
    public string? UserRefId { get; set; }
    public bool? IsActive { get; set; } = true;
    public JournalEntry? JournalEntry { get; set; }
    public OpenBranch? OpenBranch { get; set; }
    public CashVault? CashVault { get; set; }
    public List<CashManagement> CashManagements { get; set; } = [];
    public List<ForeignExchange> ForeignExchanges { get; set; } = [];
}
