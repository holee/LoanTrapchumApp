using Applications.Enumerations;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestAsPaging;

public record GetLoanRequestAsPagingQuery(string? UserId, int? BranchId, RequestStatus RequestStatus, QueryParam Param): IRequest<Pager<VLoanRequests>>;
