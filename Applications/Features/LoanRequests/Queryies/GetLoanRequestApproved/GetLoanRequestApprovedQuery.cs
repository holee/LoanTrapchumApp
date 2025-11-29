using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestBy;

public record GetLoanRequestApprovedQuery(int? BranchId, string? CustomerId, string? LoanRequestId) : IRequest<ErrorOr<ApprovedRequest>>;
