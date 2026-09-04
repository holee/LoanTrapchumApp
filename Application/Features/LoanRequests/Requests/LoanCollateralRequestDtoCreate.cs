using Application.Features.Collaterals.Dtos;

namespace Application.Features.LoanRequests.Requests;

public sealed class LoanCollateralRequestDtoCreate  
{

    public int? BranchId { get; set; } 
    public Guid? LoanRequestId { get; set; }
    public Guid? CustomerId { get; set; }
    public List<CollateralLinkDtoCreate> CollateralLinesLink { get; set; } = [];
}