using System.ComponentModel.DataAnnotations.Schema;
using Domains.Models.Loans;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Domains.Models.Collaterals;

public sealed class CollateralLink
{

    public long Id { get; set; }
    public int? BranchId { get; set; }
    public long? LoanRequestRefId { get; set; }
    public long? CollateralLineRefId { get; set; }
    public long? CollateralRefId { get; set; }
    public bool? IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
    public DateTime? CreatedDt { get; set; } = DateTime.Now;
    public DateTime? ModifiedDt { get; set; } = DateTime.Now;

    public CollateralLine? CollateralLine { get; set; }
    public LoanRequest? LoanRequest { get; set; }
}
