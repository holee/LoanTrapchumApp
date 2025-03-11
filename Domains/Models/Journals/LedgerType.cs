using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Journals;

public sealed class LedgerType
{

    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int LedgerTypeId { get; set; }
    public string? LedgerTypeCode { get; set; }
    public string? CrOrDr { get; set; }
    public int NOrder { get; set; }
    public ICollection<Ledger>? Ledgers { get; set; }
}
