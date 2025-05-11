using Domains.Models.Collaterals;

namespace Domains.Models.Loans;

public sealed class LoanCollateralRequest 
{

    public int? BranchId { get; set; } 
    
    public string? LoanRequestId { get; set; }
    
    public string? CustomerId { get; set; }
    
    public List<CollateralLink> CollateralLinesLink { get; set; } = [];
}