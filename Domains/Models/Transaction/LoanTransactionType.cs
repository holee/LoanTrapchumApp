using System.ComponentModel.DataAnnotations;

namespace Domains.Models.Transaction;
public class LoanTransactionType
{

    [Key]
    public int TransactionTypeId { get; set; }
    [StringLength(10)]
    public string TransactionCode { get; set; } = default!;
    [StringLength(50)]
    public string Description { get; set; } = string.Empty;
    public bool? IsCommission { get; set; } = false;
}
