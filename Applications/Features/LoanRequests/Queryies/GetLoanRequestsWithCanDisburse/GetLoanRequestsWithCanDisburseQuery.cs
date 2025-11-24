using Applications.Enumerations;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestsWithCanDisburse;

public  record GetLoanRequestsWithCanDisburseQuery(string? UserId, bool Candisburse, int? BranchId, RequestStatus RequestStatus, QueryParam Param)
    :IRequest<Pager<VLoanRequests>>;
