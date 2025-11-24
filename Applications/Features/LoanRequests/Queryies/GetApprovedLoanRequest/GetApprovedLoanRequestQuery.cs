using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetApprovedLoanRequest;

public record GetApprovedLoanRequestQuery(int? BranchId, string? CustomerId, string? LoanRequestId, bool Include = true)
    : IRequest<ErrorOr<VLoanRequests>>;
