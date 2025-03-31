using Domains.Models.Bases;
using Domains.Models.Journals;

namespace Domains.Models.Transaction;

public class LoanTransaction : BaseEntity
{
    public long TranId { get; set; }

    public Guid TransactionsId { get; set; }
    public Guid? TranCode { get; set; }
    public int? TranTypeRefId { get; set; }
    public string TransactionTypeName { get; set; } = string.Empty;
    public DateTime? TransactionDate { get; set; }=DateTime.Now;
    public Guid? UserRefId { get; set; }
    public bool? IsActive { get; set; } = true;
    public JournalEntry? JournalEntry { get; set; }
    public OpenBranch? OpenBranch { get; set; }
    public CashVault? CashVault { get; set; }
    public List<CashManagement> CashManagements { get; set; } = [];
    public List<ForeignExchange> ForeignExchanges { get; set; } = [];
}
