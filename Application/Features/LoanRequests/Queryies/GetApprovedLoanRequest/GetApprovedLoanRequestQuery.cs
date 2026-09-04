using Application.Responses.ViewModels.Loans.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Queryies.GetApprovedLoanRequest;

public record GetApprovedLoanRequestQuery(int? BranchId, string? CustomerId, string? LoanRequestId, bool Include = true)
    : IRequest<ErrorOr<VLoanRequests>>;
