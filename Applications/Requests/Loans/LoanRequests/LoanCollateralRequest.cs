using Domains.Models.Collaterals;

namespace Applications.Requests.Loans.LoanRequests;

public sealed class LoanCollateralRequestDtoCreate  
{

    public int? BranchId { get; set; } 
    public long? LoanRequestId { get; set; }
    public long? CustomerId { get; set; }
    public List<CollateralLinkDtoCreate> CollateralLinesLink { get; set; } = [];
}