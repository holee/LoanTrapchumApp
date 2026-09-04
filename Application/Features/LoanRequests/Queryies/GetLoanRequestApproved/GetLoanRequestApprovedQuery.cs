using Application.Responses.ViewModels.Loans.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestBy;

public record GetLoanRequestApprovedQuery(int? BranchId, string? CustomerId, string? LoanRequestId) : IRequest<ErrorOr<ApprovedRequest>>;
