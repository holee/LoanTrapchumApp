using Domains.Models.Collaterals;

namespace Domains.Models.Loans;

public sealed class LoanCollateralRequest 
{

    public int? BranchId { get; set; } 
    public long? LoanRequestId { get; set; }
    public long? CustomerId { get; set; }
    public List<CollateralLinkDtoCreate> CollateralLinesLink { get; set; } = [];
}