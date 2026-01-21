using Domains.Models.Collaterals;

namespace Domains.Models.Loans;

public sealed class LoanCollateralRequest 
{
    public LoanCollateralRequest()
    {
        BranchId = 0;
        CustomerId=string.Empty;
        LoanRequestId=string.Empty;
        CollateralLinesLink = [];
    }
    public LoanCollateralRequest(int? branchId, string? loanRequestId, string? customerId)
    {
        BranchId = branchId;
        LoanRequestId = loanRequestId;
        CustomerId = customerId;
    }

    public int? BranchId { get; set; } 
    
    public string? LoanRequestId { get; set; }
    
    public string? CustomerId { get; set; }

    public string? LoanRefId { get; set; }

    public List<CollateralLink> CollateralLinesLink { get; set; } = [];
}