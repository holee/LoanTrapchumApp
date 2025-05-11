using Applications.Requests.Lists.Collaterals;
using Domains.Models.Collaterals;

namespace Applications.Requests.Loans.LoanRequests;

public sealed class LoanCollateralRequestDtoCreate  
{

    public int? BranchId { get; set; } 
    public Guid? LoanRequestId { get; set; }
    public Guid? CustomerId { get; set; }
    public List<CollateralLinkDtoCreate> CollateralLinesLink { get; set; } = [];
}