using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestById;

public record GetLoanRequestByIdQuery(int? BranchId, string? CustomerId, string? LoanRequestId, bool Include) : IRequest<ErrorOr<VLoanRequests>>;
