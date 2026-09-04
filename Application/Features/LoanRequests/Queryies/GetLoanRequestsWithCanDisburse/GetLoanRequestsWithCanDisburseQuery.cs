using Application.Enumerations;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestsWithCanDisburse;

public  record GetLoanRequestsWithCanDisburseQuery(string? UserId, bool Candisburse, int? BranchId, RequestStatus RequestStatus, QueryParam Param)
    :IRequest<Pager<VLoanRequests>>;
