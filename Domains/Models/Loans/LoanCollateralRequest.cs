using Domains.Models.Collaterals;

namespace Domains.Models.Loans;

public sealed class LoanCollateralRequest 
{

    public int? BranchId { get; set; } 
    public Guid? LoanRequestId { get; set; }
    public Guid? CustomerId { get; set; }
    public List<CollateralLinkDtoCreate> CollateralLinesLink { get; set; } = [];
}