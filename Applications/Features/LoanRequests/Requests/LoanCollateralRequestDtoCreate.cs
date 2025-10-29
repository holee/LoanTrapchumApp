using Applications.Features.Collaterals.Dtos;

namespace Applications.Features.LoanRequests.Requests;

public sealed class LoanCollateralRequestDtoCreate  
{

    public int? BranchId { get; set; } 
    public Guid? LoanRequestId { get; set; }
    public Guid? CustomerId { get; set; }
    public List<CollateralLinkDtoCreate> CollateralLinesLink { get; set; } = [];
}